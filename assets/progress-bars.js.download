'use strict';
/**
 * @author Batch Themes Ltd.
 */
(function() {
    $(function() {
        if (!element_exists('#ui-elements-progress-bars')) {
            return false;
        }
        setTimeout(function() {
            $('.progress-bar').each(function() {
                var value = $(this).attr('data-value');
                $(this).animate({
                    width: value + '%'
                }, 1000);
            });
        }, 1000);
    });
})();
