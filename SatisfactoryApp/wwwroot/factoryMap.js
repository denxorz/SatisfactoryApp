let graphviz = null;

window.loadGraphviz = async function () {
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

window.renderDotGraph = async function (dotContent) {
  if (!graphviz) {
    await window.loadGraphviz();
  }
  return await graphviz.dot(dotContent);
};

window.getElementBoundingClientRect = function (selector) {
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
