var Sdk = window.Sdk || {};
(
    function () {
        // Form OnLoad: Retrieves lookup details and shows notifications
        this.formOnLoad = function (executionContext) {
            var formContext = executionContext.getFormContext();
            var lookupArray = formContext.getAttribute("parentcustomerid").getValue(); //lookup field returns an array 

            if (lookupArray != null && lookupArray[0] != null) {
                var guidofAccount = lookupArray[0].id;
                var nameofAccount = lookupArray[0].name;
                var entitytype = lookupArray[0].entityType;

                // Note: Images show a slight typo 'setFormNotificaion' 
                // Correct API is: setFormNotification
                formContext.ui.setFormNotification("GUID of the Account is " + guidofAccount, "INFO", "1");
                formContext.ui.setFormNotification("Name of the Account is " + nameofAccount, "INFO", "2");
                formContext.ui.setFormNotification("Entity Type is " + entitytype, "INFO", "3");
            }
        };

        // First Name OnChange Placeholder
        this.firstnameOnChange = function (executionContext) {
            // Logic for firstname change
        };

        // Shipping Method OnChange: Toggles field locking
        this.shippingMethodOnChange = function (executionContext) {
            var formContext = executionContext.getFormContext();
            var shippingMethod = formContext.getAttribute("address1_shippingmethodcode").getText();

            if (shippingMethod == "FedEx") {
                formContext.getControl("address1_freighttermscode").setDisabled(true);
            } else {
                formContext.getControl("address1_freighttermscode").setDisabled(false);
            }

        }

        //simple code snippets
        this.FormOnLoad = function (executionContext) {
            var formContext = executionContext.getFormContext();
            var firstName = formContext.getAttribute("firstname").getValue();

            // Check if firstName has a value before alerting
            if (firstName) {
                alert("Hello this contact is " + firstName);
            }
        }

        this.FirstnameOnChange = function (executionContext) {
            var formContext = executionContext.getFormContext();
            var firstName = formContext.getAttribute("firstname").getValue();

            alert("Hello, you are changing first name to " + firstName);
        }
).call(Sdk);

