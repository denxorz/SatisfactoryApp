globalThis.getElementBoundingClientRect = function (selector) {
  const element = document.querySelector(selector);
  if (!element) return null;
  const rect = element.getBoundingClientRect();
  return {
    left: rect.left,
    top: rect.top,
    width: rect.width,
    height: rect.height
  };
};

const mapInteractions = new Map();

function clamp(value, min, max) {
  return Math.min(max, Math.max(min, value));
}

function applyTransform(state) {
  state.wrapper.style.transform = `translate(${state.translateX}px, ${state.translateY}px) scale(${state.scale})`;
  state.container.style.setProperty('--map-translate-x', `${state.translateX}px`);
  state.container.style.setProperty('--map-translate-y', `${state.translateY}px`);
  state.container.style.setProperty('--map-scale', state.scale);
}

function setDraggingClass(state, isDragging) {
  if (!state.container) return;
  if (isDragging) {
    state.container.classList.add('map-dragging');
  } else {
    state.container.classList.remove('map-dragging');
  }
}

function updateSuppressClick(state) {
  state.suppressClickUntil = Date.now() + 200;
}

function zoomAt(state, clientX, clientY, nextScale) {
  const rect = state.container.getBoundingClientRect();
  const pointerX = clientX - rect.left;
  const pointerY = clientY - rect.top;

  const contentX = (pointerX - state.translateX) / state.scale;
  const contentY = (pointerY - state.translateY) / state.scale;

  state.scale = nextScale;
  state.translateX = pointerX - contentX * state.scale;
  state.translateY = pointerY - contentY * state.scale;

  applyTransform(state);
}

function handleWheel(state, event) {
  event.preventDefault();
  const zoomFactor = Math.exp(-event.deltaY * 0.001);
  const nextScale = clamp(state.scale * zoomFactor, state.minScale, state.maxScale);
  if (nextScale === state.scale) return;
  zoomAt(state, event.clientX, event.clientY, nextScale);
}

function distanceBetween(a, b) {
  const dx = a.x - b.x;
  const dy = a.y - b.y;
  return Math.hypot(dx, dy);
}

function midpointBetween(a, b) {
  return {
    x: (a.x + b.x) / 2,
    y: (a.y + b.y) / 2
  };
}

function createMapState(container, wrapper, options) {
  return {
    container,
    wrapper,
    scale: options?.startScale ?? 1,
    minScale: options?.minScale ?? 0.6,
    maxScale: options?.maxScale ?? 5,
    translateX: 0,
    translateY: 0,
    isDragging: false,
    didDrag: false,
    dragStart: null,
    activePointers: new Map(),
    pinchStart: null,
    suppressClickUntil: 0,
    wheelHandler: null,
    pointerDownHandler: null,
    pointerMoveHandler: null,
    pointerUpHandler: null
  };
}

globalThis.initMapZoomPan = function (containerSelector, wrapperSelector, options) {
  if (mapInteractions.has(containerSelector)) return;
  const container = document.querySelector(containerSelector);
  const wrapper = document.querySelector(wrapperSelector);
  if (!container || !wrapper) return;

  const state = createMapState(container, wrapper, options);
  applyTransform(state);

  state.wheelHandler = (event) => handleWheel(state, event);

  state.pointerDownHandler = (event) => {
    if (event.pointerType === 'mouse' && event.button !== 0) return;
    event.preventDefault();
    container.setPointerCapture(event.pointerId);
    state.activePointers.set(event.pointerId, { x: event.clientX, y: event.clientY });

    if (state.activePointers.size === 1) {
      state.isDragging = true;
      state.didDrag = false;
      state.dragStart = {
        x: event.clientX,
        y: event.clientY,
        translateX: state.translateX,
        translateY: state.translateY
      };
      setDraggingClass(state, true);
    }

    if (state.activePointers.size === 2) {
      const pointers = Array.from(state.activePointers.values());
      const startDistance = distanceBetween(pointers[0], pointers[1]);
      const midpoint = midpointBetween(pointers[0], pointers[1]);
      state.pinchStart = {
        distance: startDistance,
        scale: state.scale,
        midpointX: midpoint.x,
        midpointY: midpoint.y,
        contentX: (midpoint.x - state.container.getBoundingClientRect().left - state.translateX) / state.scale,
        contentY: (midpoint.y - state.container.getBoundingClientRect().top - state.translateY) / state.scale
      };
      state.didDrag = true;
    }
  };

  state.pointerMoveHandler = (event) => {
    if (!state.activePointers.has(event.pointerId)) return;
    state.activePointers.set(event.pointerId, { x: event.clientX, y: event.clientY });

    if (state.activePointers.size === 1 && state.dragStart) {
      const dx = event.clientX - state.dragStart.x;
      const dy = event.clientY - state.dragStart.y;
      if (Math.abs(dx) > 3 || Math.abs(dy) > 3) {
        state.didDrag = true;
      }
      state.translateX = state.dragStart.translateX + dx;
      state.translateY = state.dragStart.translateY + dy;
      applyTransform(state);
    }

    if (state.activePointers.size === 2 && state.pinchStart) {
      const pointers = Array.from(state.activePointers.values());
      const distance = distanceBetween(pointers[0], pointers[1]);
      const midpoint = midpointBetween(pointers[0], pointers[1]);
      const nextScale = clamp(state.pinchStart.scale * (distance / state.pinchStart.distance), state.minScale, state.maxScale);
      state.scale = nextScale;
      state.translateX = midpoint.x - state.container.getBoundingClientRect().left - state.pinchStart.contentX * state.scale;
      state.translateY = midpoint.y - state.container.getBoundingClientRect().top - state.pinchStart.contentY * state.scale;
      applyTransform(state);
      state.didDrag = true;
    }
  };

  state.pointerUpHandler = (event) => {
    if (state.activePointers.has(event.pointerId)) {
      state.activePointers.delete(event.pointerId);
    }

    if (state.activePointers.size < 2) {
      state.pinchStart = null;
    }

    if (state.activePointers.size === 0) {
      if (state.didDrag) {
        updateSuppressClick(state);
      }
      state.isDragging = false;
      state.dragStart = null;
      setDraggingClass(state, false);
    }
  };

  container.addEventListener('wheel', state.wheelHandler, { passive: false });
  container.addEventListener('pointerdown', state.pointerDownHandler);
  container.addEventListener('pointermove', state.pointerMoveHandler);
  container.addEventListener('pointerup', state.pointerUpHandler);
  container.addEventListener('pointercancel', state.pointerUpHandler);

  mapInteractions.set(containerSelector, state);
};

globalThis.disposeMapZoomPan = function (containerSelector) {
  const state = mapInteractions.get(containerSelector);
  if (!state) return;

  state.container.removeEventListener('wheel', state.wheelHandler);
  state.container.removeEventListener('pointerdown', state.pointerDownHandler);
  state.container.removeEventListener('pointermove', state.pointerMoveHandler);
  state.container.removeEventListener('pointerup', state.pointerUpHandler);
  state.container.removeEventListener('pointercancel', state.pointerUpHandler);

  mapInteractions.delete(containerSelector);
};

globalThis.resetMapZoomPan = function (containerSelector) {
  const state = mapInteractions.get(containerSelector);
  if (!state) return;
  state.scale = state.minScale;
  state.translateX = 0;
  state.translateY = 0;
  applyTransform(state);
};

globalThis.shouldIgnoreMapClick = function (containerSelector) {
  const state = mapInteractions.get(containerSelector);
  if (!state) return false;
  return Date.now() < state.suppressClickUntil;
};

globalThis.getMapPoint = function (containerSelector, wrapperSelector, clientX, clientY) {
  const state = mapInteractions.get(containerSelector);
  const container = state?.container ?? document.querySelector(containerSelector);
  const wrapper = state?.wrapper ?? document.querySelector(wrapperSelector);
  if (!container || !wrapper) return null;
  const rect = container.getBoundingClientRect();
  const scale = state?.scale ?? 1;
  const translateX = state?.translateX ?? 0;
  const translateY = state?.translateY ?? 0;
  const x = (clientX - rect.left - translateX) / scale;
  const y = (clientY - rect.top - translateY) / scale;
  return {
    x,
    y,
    width: wrapper.clientWidth,
    height: wrapper.clientHeight
  };
};

globalThis.downloadJsonFile = function (jsonContent, filename) {
  const blob = new Blob([jsonContent], { type: 'application/json' });
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = filename;
  document.body.appendChild(link);
  link.click();
  link.remove();
  URL.revokeObjectURL(url);
};

let jsonMessageHandler = null;

globalThis.registerJsonMessageHandler = function (dotNetRef) {
  if (jsonMessageHandler) return;

  jsonMessageHandler = function (event) {
    const message = event?.data ?? null;
    if (message?.type !== 'statisfactory-json') return;

    const payload = message.payload || {};
    const jsonContent = payload.json || message.json;
    if (typeof jsonContent !== 'string') return;

    const filename = payload.filename || message.filename || null;
    dotNetRef.invokeMethodAsync('LoadJsonFromMessage', jsonContent, filename);
  };

  globalThis.addEventListener('message', jsonMessageHandler);
  try {
    const parentWindow = globalThis.parent;
    if (parentWindow) {
      let targetOrigin = 'null';
      if (document.referrer) {
        try {
          targetOrigin = new URL(document.referrer).origin;
        } catch {
          targetOrigin = 'null';
        }
      }
      parentWindow.postMessage({ type: 'statisfactory-ready' }, targetOrigin);
    }
  } catch {
    // Ignore cross-origin errors; readiness is best-effort.
  }
};

globalThis.unregisterJsonMessageHandler = function () {
  if (!jsonMessageHandler) return;
  globalThis.removeEventListener('message', jsonMessageHandler);
  jsonMessageHandler = null;
};