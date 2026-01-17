let graphviz = null;

globalThis.loadGraphviz = async function () {
  if (!graphviz) {
    try {
      const { Graphviz } = await import('https://cdn.jsdelivr.net/npm/@hpcc-js/wasm@2/dist/index.js');
      graphviz = await Graphviz.load();
    } catch (error) {
      console.error('Failed to load Graphviz from CDN:', error);
      throw error;
    }
  }
};

globalThis.renderDotGraph = async function (dotContent) {
  if (!graphviz) {
    await globalThis.loadGraphviz();
  }
  return await graphviz.dot(dotContent);
};

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