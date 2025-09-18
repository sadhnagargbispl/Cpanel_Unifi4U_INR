'use strict';
/**
 * @author Batch Themes Ltd.
 */
(function() {
    $(function() {
        $('.right-sidebar .set-layout').on('click', function(e) {
            e.preventDefault();
            var action = $(this).attr('data-action');
            var value = $(this).attr('data-value');
            if (action == 'collapsed') {
                $('body').attr('data-collapsed', value);
                $('body').trigger('toggle-collapsed');
            }
            return false;
        });
        $('.right-sidebar .color-block').on('click', function(e) {
            e.preventDefault();
            var action = $(this).attr('data-action');
            var value = $(this).attr('data-value');
            if (action == 'background') {
                $('body').attr('data-background', value);
                $('body').trigger('changed:background');
            }
            if (action == 'navbar') {
                $('body').attr('data-navbar', value);
                $('body').trigger('changed:navbar');
                $('body').attr('data-logo', value);
                $('body').trigger('changed:logo');
            }
            if (action == 'top-navigation') {
                $('body').attr('data-top-navigation', value);
                $('body').trigger('changed:top-navigation');
            }
            if (action == 'sidebar') {
                $('body').attr('data-sidebar', value);
                $('body').trigger('changed:sidebar');
            }
            return false;
        });
    });
})();
