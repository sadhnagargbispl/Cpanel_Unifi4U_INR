'use strict';
/**
 * @author Batch Themes Ltd.
 */
(function() {
    $(function() {
        if (!element_exists('#notifications-toastr')) {
            return false;
        }
        $('.btn-toast').on('click', function() {
            var type = $(this).data('type');
            if (type === 'success') {
                toastr.options = {
                    iconClass: '',
                    positionClass: 'toast-top-right'
                };
                toastr.success('Great idea!');
            }
            if (type === 'warning') {
                toastr.options = {
                    iconClass: '',
                    positionClass: 'toast-bottom-right'
                };
                toastr.warning('Warning!');
            }
            if (type === 'danger') {
                toastr.options = {
                    iconClass: '',
                    positionClass: 'toast-bottom-left'
                };
                toastr.error('Danger!');
            }
            if (type === 'info') {
                toastr.options = {
                    iconClass: '',
                    positionClass: 'toast-top-left'
                };
                toastr.info('Excellent!');
            }
        });
    });
})();
