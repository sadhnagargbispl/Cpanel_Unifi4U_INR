'use strict';
/**
 * @author Batch Themes Ltd.
 */
(function() {
    $(function() {
        if (!element_exists('#forms-sliders')) {
            return false;
        }
        var colors = ['primary', 'secondary', 'info', 'success', 'warning', 'danger'];
        for (var i in colors) {
            var slider = 'slider-' + colors[i];
            noUiSlider.create(document.getElementById(slider), {
                start: [random(10, 30), random(70, 90)],
                connect: true,
                range: {
                    'min': 0,
                    'max': 100
                }
            });
        }
    });
})();
