using System.Web;
using System.Web.Optimization;

namespace DocumentManager.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/common/libraries").Include(
                      "~/assets/js-core/jquery-core.js",
                      "~/assets/js-core/jquery-ui-core.js",
                      "~/assets/js-core/jquery-ui-widget.js",
                      "~/assets/js-core/jquery-ui-mouse.js",
                      "~/assets/js-core/jquery-ui-position.js",
                      "~/assets/js-core/transition.js",
                      "~/assets/js-core/modernizr.js",
                      "~/assets/datatables/jquery.dataTables.min.js",
                      "~/assets/datatables/dataTables.buttons.min.js",
                      "~/assets/datatables/dataTables.checkboxes.min.js",
                      "~/assets/datatables/jszip.min.js",
                      "~/assets/datatables/pdfmake.min.js",
                      "~/assets/datatables/vfs_fonts.js",
                      "~/assets/datatables/buttons.html5.min.js",
                      "~/assets/widgets/skycons/skycons.js",
                      "~/assets/widgets/owlcarousel/owlcarousel.js",
                      "~/assets/widgets/owlcarousel/owlcarousel-demo.js",
                      "~/assets/widgets/dropdown/dropdown.js",
                      "~/assets/widgets/tooltip/tooltip.js",
                      "~/assets/widgets/popover/popover.js",
                      "~/assets/widgets/progressbar/progressbar.js",
                      "~/assets/widgets/button/button.js",
                      "~/assets/widgets/collapse/collapse.js",
                      "~/assets/widgets/superclick/superclick.js",
                      "~/assets/widgets/input-switch/inputswitch-alt.js",
                      "~/assets/widgets/slimscroll/slimscroll.js",
                      "~/assets/widgets/slidebars/slidebars.js",
                      "~/assets/widgets/slidebars/slidebars-demo.js",
                      "~/assets/widgets/content-box/contentbox.js",
                      "~/assets/widgets/material/material.js",
                      "~/assets/widgets/material/ripples.js",
                      "~/assets/widgets/overlay/overlay.js",
                      "~/assets/widgets/noty-notifications/noty.js",
                      "~/assets/widgets/parsley/parsley.js",
                      "~/Scripts/Utility/configFile.js",
                      "~/Scripts/Utility/messageBox.js",
                       "~/Scripts/Login/Login.js"));

            bundles.Add(new ScriptBundle("~/resetpassword").Include(
                    "~/Scripts/Password/PasswordReset.js"
                    ));

            bundles.Add(new ScriptBundle("~/layout").Include(
                    "~/Scripts/Login/Layout.js",
                    "~/Scripts/DynamicMenu/Layout.js"
                    ));

            bundles.Add(new ScriptBundle("~/dashboard").Include(
                    "~/Scripts/DynamicMenu/Dashboard.js"
                    ));

            bundles.Add(new ScriptBundle("~/fullscreen").Include(
                "~/assets/widgets/screenfull/screenfull.js",
                      "~/assets/themes/admin/layout.js",
                      "~/assets/js-init/widgets-init.js"));

            bundles.Add(new ScriptBundle("~/changepassword").Include(
                   "~/Scripts/Password/ChangePassword.js"
                   ));

            bundles.Add(new ScriptBundle("~/unlockscreen").Include(
                   "~/Scripts/Password/UnlockScreen.js"
                   ));

            bundles.Add(new ScriptBundle("~/addfunctions").Include(
                      "~/Scripts/Function/AddFunction.js"
                      ));

            bundles.Add(new ScriptBundle("~/updatefunctions").Include(
                     "~/Scripts/Function/UpdateFunction.js"
                     ));

            bundles.Add(new ScriptBundle("~/viewfunctions").Include(
                      "~/Scripts/Function/ViewFunction.js"
                      ));

            bundles.Add(new ScriptBundle("~/addroles").Include(
                      "~/Scripts/Role/AddRole.js"
                      ));

            bundles.Add(new ScriptBundle("~/updateroles").Include(
                      "~/Scripts/Role/UpdateRole.js"
                      ));

            bundles.Add(new ScriptBundle("~/viewroles").Include(
                      "~/Scripts/Role/ViewRole.js"
                      ));

            bundles.Add(new ScriptBundle("~/addusers").Include(
                     "~/Scripts/User/AddUser.js"
                     ));

            bundles.Add(new ScriptBundle("~/updateusers").Include(
                    "~/Scripts/User/UpdateUser.js"
                    ));

            bundles.Add(new ScriptBundle("~/viewusers").Include(
                      "~/Scripts/User/ViewUser.js"
                      ));

            bundles.Add(new ScriptBundle("~/addcriterias").Include(
                     "~/Scripts/CatalogueCriteria/AddCriteria.js"
                     ));

            bundles.Add(new ScriptBundle("~/updatecriterias").Include(
                    "~/Scripts/CatalogueCriteria/UpdateCriteria.js"
                    ));

            bundles.Add(new ScriptBundle("~/viewcriterias").Include(
                      "~/Scripts/CatalogueCriteria/ViewCriteria.js"
                      ));

            bundles.Add(new ScriptBundle("~/addlocations").Include(
                     "~/Scripts/PhysicalLocation/AddLocation.js"
                     ));

            bundles.Add(new ScriptBundle("~/updatelocations").Include(
                    "~/Scripts/PhysicalLocation/UpdateLocation.js"
                    ));

            bundles.Add(new ScriptBundle("~/viewlocations").Include(
                      "~/Scripts/PhysicalLocation/ViewLocation.js"
                      ));

            bundles.Add(new StyleBundle("~/common/css").Include(
                      "~/assets/helpers/animate.css",
                      "~/assets/helpers/boilerplate.css",
                      "~/assets/helpers/border-radius.css",
                      "~/assets/helpers/grid.css",
                      "~/assets/helpers/page-transitions.css",
                      "~/assets/helpers/spacing.css",
                      "~/assets/helpers/typography.css",
                      "~/assets/helpers/utils.css",
                      "~/assets/helpers/colors.css",
                      "~/assets/material/ripple.css",
                      "~/assets/elements/badges.css",
                      "~/assets/elements/buttons.css",
                      "~/assets/elements/content-box.css",
                      "~/assets/elements/dashboard-box.css",
                      "~/assets/elements/forms.css",
                      "~/assets/elements/loading-indicators.css",
                      "~/assets/elements/menus.css",
                      "~/assets/elements/panel-box.css",
                      "~/assets/elements/tile-box.css",
                      "~/assets/elements/timeline.css",
                      "~/assets/icons/fontawesome/fontawesome.css",
                      "~/assets/icons/linecons/linecons.css",
                      "~/assets/icons/spinnericon/spinnericon.css",
                      "~/assets/widgets/accordion-ui/accordion.css",
                      "~/assets/widgets/chosen/chosen.css",
                      "~/assets/widgets/carousel/carousel.css",
                      "~/assets/widgets/colorpicker/colorpicker.css",
                       "~/assets/datatables/jquery.dataTables.min.css",
                      "~/assets/datatables/buttons.dataTables.min.css",
                      "~/assets/datatables/dataTables.checkboxes.css",
                      "~/assets/widgets/datepicker/datepicker.css",
                      "~/assets/widgets/datepicker-ui/datepicker.css",
                      "~/assets/widgets/daterangepicker/daterangepicker.css",
                      "~/assets/widgets/dialog/dialog.css",
                      "~/assets/widgets/dropdown/dropdown.css",
                      "~/assets/widgets/dropzone/dropzone.css",
                      "~/assets/widgets/file-input/fileinput.css",
                      "~/assets/widgets/multi-upload/fileupload.css",
                      "~/assets/widgets/nestable/nestable.css",
                      "~/assets/widgets/noty-notifications/noty.css",
                      "~/assets/widgets/popover/popover.css",
                      "~/assets/widgets/dropzone/dropzone.css",
                      "~/assets/widgets/slidebars/slidebars.css",
                      "~/assets/widgets/slider-ui/slider.css",
                      "~/assets/widgets/tocify/tocify.css",
                      "~/assets/widgets/tooltip/tooltip.css",
                      "~/assets/snippets/user-profile.css",
                      "~/assets/snippets/mobile-navigation.css",
                      "~/assets/themes/admin/layout.css",
                      "~/assets/themes/admin/color-schemes/default.css",
                      "~/assets/themes/components/default.css",
                      "~/assets/themes/components/border-radius.css",
                      "~/assets/helpers/responsive-elements.css",
                      "~/assets/helpers/admin-responsive.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
