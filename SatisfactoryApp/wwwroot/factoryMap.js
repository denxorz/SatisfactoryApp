// Factory Map JavaScript interop functions
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

window.mergeFactoryMapImages = async function (svgFiltered, svgNonFiltered, dotNetRef) {
  try {
    const canvas = document.createElement('canvas');
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const bgImg = new Image();
    bgImg.crossOrigin = 'anonymous';

    bgImg.onload = () => {
      canvas.width = bgImg.width;
      canvas.height = bgImg.height;

      ctx.filter = 'grayscale(0.4) contrast(0.8) brightness(1.1)';
      ctx.globalAlpha = 0.4;
      ctx.drawImage(bgImg, 0, 0, canvas.width, canvas.height);

      ctx.globalAlpha = 1.0;
      ctx.filter = 'none';
      loadAndMergeSvgs(svgFiltered, svgNonFiltered, ctx, canvas, dotNetRef);
    };

    bgImg.onerror = (error) => {
      console.error('Failed to load biome map image:', error);
      // Continue without background image
      canvas.width = 1920;
      canvas.height = 1080;
      loadAndMergeSvgs(svgFiltered, svgNonFiltered, ctx, canvas, dotNetRef);
    };

    bgImg.src = '/1920px-Biome_Map.jpg';
  } catch (error) {
    console.error('Failed to merge images:', error);
  }
};

function loadAndMergeSvgs(svgFiltered, svgNonFiltered, ctx, canvas, dotNetRef) {
  const loadSvgImage = (svgContent) => {
    return new Promise((resolve, reject) => {
      const svgBlob = new Blob([svgContent], { type: 'image/svg+xml' });
      const svgUrl = URL.createObjectURL(svgBlob);
      const svgImg = new Image();
      svgImg.crossOrigin = 'anonymous';

      svgImg.onload = () => {
        URL.revokeObjectURL(svgUrl);
        resolve(svgImg);
      };
      svgImg.onerror = reject;
      svgImg.src = svgUrl;
    });
  };

  Promise.all([loadSvgImage(svgFiltered), loadSvgImage(svgNonFiltered)])
    .then(([filteredImg, nonFilteredImg]) => {
      ctx.globalAlpha = 0.3;
      ctx.drawImage(filteredImg, 0, 0, canvas.width, canvas.height);

      ctx.globalAlpha = 1.0;
      ctx.drawImage(nonFilteredImg, 0, 0, canvas.width, canvas.height);
      canvas.toBlob((blob) => {
        if (blob) {
          const url = URL.createObjectURL(blob);
          dotNetRef.invokeMethodAsync('SetMergedImageUrl', url);
        }
      }, 'image/png');
    })
    .catch((error) => {
      console.error('Failed to load SVG images:', error);
    });
}

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

