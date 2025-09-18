'use strict';
/**
 * @author Batch Themes Ltd.
 */
(function() {
    $(function() {
        if (!element_exists('#charts-peity')) {
            return false;
        }
        var colors = bootstrap_colors();
        var palette = get_palette();
        /* line charts */
        peityLine('.medium-line-1', 32, 96, colors.primary);
        peityLine('.medium-line-2', 32, 96, colors.secondary);
        peityLine('.medium-line-3', 32, 96, colors.info);
        peityLine('.medium-line-4', 32, 96, colors.success);
        peityLine('.medium-line-5', 32, 96, colors.warning);
        peityLine('.medium-line-6', 32, 96, colors.danger);
        /* area charts */
        peityArea('.medium-area-1', 32, 96, colors.primary);
        peityArea('.medium-area-2', 32, 96, colors.secondary);
        peityArea('.medium-area-3', 32, 96, colors.info);
        peityArea('.medium-area-4', 32, 96, colors.success);
        peityArea('.medium-area-5', 32, 96, colors.warning);
        peityArea('.medium-area-6', 32, 96, colors.danger);
        /* bar charts */
        peityBar('.medium-bar-1', 32, 96, colors.primary);
        peityBar('.medium-bar-2', 32, 96, colors.secondary);
        peityBar('.medium-bar-3', 32, 96, colors.info);
        peityBar('.medium-bar-4', 32, 96, colors.success);
        peityBar('.medium-bar-5', 32, 96, colors.warning);
        peityBar('.medium-bar-6', 32, 96, colors.danger);
        /* pie charts */
        peityPie('.medium-pie-1', 64, [palette['border-color'], colors.primary]);
        peityPie('.medium-pie-2', 64, [palette['border-color'], colors.secondary]);
        peityPie('.medium-pie-3', 64, [palette['border-color'], colors.info]);
        peityPie('.medium-pie-4', 64, [palette['border-color'], colors.success]);
        peityPie('.medium-pie-5', 64, [palette['border-color'], colors.warning]);
        peityPie('.medium-pie-6', 64, [palette['border-color'], colors.danger]);
        /* donut charts */
        peityDonut('.medium-donut-1', 64, [palette['border-color'], colors.primary]);
        peityDonut('.medium-donut-2', 64, [palette['border-color'], colors.secondary]);
        peityDonut('.medium-donut-3', 64, [palette['border-color'], colors.info]);
        peityDonut('.medium-donut-4', 64, [palette['border-color'], colors.success]);
        peityDonut('.medium-donut-5', 64, [palette['border-color'], colors.warning]);
        peityDonut('.medium-donut-6', 64, [palette['border-color'], colors.danger]);
        $.fn.peity.defaults.pie = {
            delimiter: null,
            fill: [colors.primary, colors.secondary, colors.info],
            height: null,
            radius: 8,
            width: null
        };
        $.fn.peity.defaults.donut = {
            delimiter: null,
            fill: [colors.primary, colors.secondary, colors.info],
            height: null,
            innerRadius: null,
            radius: 8,
            width: null
        };
        $.fn.peity.defaults.line = {
            delimiter: ',',
            fill: colors.primary,
            height: 16,
            max: null,
            min: 0,
            stroke: colors.primary,
            strokeWidth: 1,
            width: 32
        };
        $.fn.peity.defaults.bar = {
            delimiter: ',',
            fill: [colors.primary],
            height: 16,
            max: null,
            min: 0,
            padding: 0.1,
            width: 32
        };
    });
})();
