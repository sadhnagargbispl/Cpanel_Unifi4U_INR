'use strict';
/**
 * @author Batch Themes Ltd.
 */
(function() {
    $(function() {
        if (!element_exists('#maps-vector-maps')) {
            return false;
        }
        var colors = bootstrap_colors();
        var palette = get_palette();
        var defaultFill = palette['even-color'];
        var altFill = palette['even-color'];
        var labelFill = palette['odd-color'];
        var borderColor = palette['border-color'];
        worldMap('world', defaultFill, borderColor, altFill, labelFill, colors.danger, colors.warning, colors.info, colors.success);
        usaMap('usa', defaultFill, borderColor, labelFill);
        worldMapWithBubbles('bubble-map', defaultFill, altFill, colors.warning, colors.warning, colors.warning);
        function onResizeEnd() {
            $('#world').html('');
            $('#usa').html('');
            $('#bubble-map').html('');
            palette = get_palette();
            defaultFill = palette['even-color'];
            altFill = palette['even-color'];
            labelFill = palette['odd-color'];
            borderColor = palette['border-color'];
            worldMap('world', defaultFill, borderColor, altFill, labelFill, colors.danger, colors.warning, colors.info, colors.success);
            usaMap('usa', defaultFill, borderColor, labelFill);
            worldMapWithBubbles('bubble-map', defaultFill, borderColor, colors.warning, colors.warning, colors.warning);
        }
        var resizeTimeout;
        $(window).on('resize', function() {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(onResizeEnd, 500);
        });
        $('body').on('changed:background', function(e, key, value) {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(onResizeEnd, 500);
        });
        $('body').on('toggle:collapsed', function(e, key, value) {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(onResizeEnd, 500);
        });
    });
})();
