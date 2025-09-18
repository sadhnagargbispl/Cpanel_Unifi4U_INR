'use strict';
/**
 * @author Batch Themes Ltd.
 * @description Mousetrap helpers to control layout settings with the keyboard. This can be removed in production
 */
(function() {
    $(function() {
        var storage = Storages.localStorage;
        var backgrounds = ['light', 'dark', 'indigo', 'blue-grey'];
        Mousetrap.bind('ctrl+1', function() {
            var background = $('body').attr('data-background');
            var index = _.indexOf(backgrounds, background);
            var nextBackground = backgrounds[index + 1] ? backgrounds[index + 1] : backgrounds[0];
            $('body').attr('data-background', nextBackground);
            $('body').trigger('changed:background');
        });
        var navbars = backgrounds.slice();
        Mousetrap.bind('ctrl+2', function() {
            var navbar = $('body').attr('data-navbar');
            var index = _.indexOf(navbars, navbar);
            var nextNavbar = navbars[index + 1] ? navbars[index + 1] : navbars[0];
            $('body').attr('data-navbar', nextNavbar);
            $('body').trigger('changed:navbar');
            $('body').attr('data-logo', nextNavbar);
            $('body').trigger('changed:logo');
        });
        var sidebars = backgrounds.slice();
        Mousetrap.bind('ctrl+3', function() {
            var sidebar = $('body').attr('data-sidebar');
            var index = _.indexOf(sidebars, sidebar);
            var nextSidebar = sidebars[index + 1] ? sidebars[index + 1] : sidebars[0];
            $('body').attr('data-sidebar', nextSidebar);
            $('body').trigger('changed:sidebar');
        });
        var topNavigations = backgrounds.slice();
        Mousetrap.bind('ctrl+4', function() {
            var topNavigation = $('body').attr('data-top-navigation');
            var index = _.indexOf(topNavigations, topNavigation);
            var nextNavbar = topNavigations[index + 1] ? topNavigations[index + 1] : topNavigations[0];
            $('body').attr('data-top-navigation', nextNavbar);
            $('body').trigger('changed:top-navigation');
        });
        var logos = backgrounds.slice();
        Mousetrap.bind('ctrl+5', function() {
            var logo = $('body').attr('data-logo');
            var index = _.indexOf(logos, logo);
            var nextLogo = logos[index + 1] ? logos[index + 1] : logos[0];
            $('body').attr('data-logo', nextLogo);
            $('body').trigger('changed:logo');
        });
        Mousetrap.bind('ctrl+6', function() {
            $('.right-sidebar-outer').toggleClass('show-from-right');
            var layout = $('body').data('layout');
            if ($('.right-sidebar-outer').hasClass('show-from-right')) {
                $('.right-sidebar-backdrop').toggleClass('fade in');
            } else {
                $('.right-sidebar-backdrop')
                    .removeClass('fade')
                    .removeClass('in');
            }
        });
        Mousetrap.bind('ctrl+7', function() {
            var collapsed = $('body').attr('data-collapsed') === 'true' ? true : false;
            $('body').attr('data-collapsed', !collapsed);
            $('body').trigger('changed:collapsed');
        });
        Mousetrap.bind('ctrl+8', function() {
            var offCanvas = $('body').attr('data-off-canvas') === 'true' ? true : false;
            $('body').attr('data-off-canvas', !offCanvas);
            //$('body').trigger('changed:off-canvas');
        });
        Mousetrap.bind('ctrl+9', function() {
            storage.removeAll();
        });
    });
})();
