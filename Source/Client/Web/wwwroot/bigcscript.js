window.addEventListener('load', function () {
    var foundElement = false;
    var foundRadioButtons = false;

    var cartUrl = '/api/storefront/carts';
    var monriApiUrl = 'https://monribigcomm.wspay.info';
    var isLoadingClassName = 'is-loading';
    var customMonriButton = document.createElement("button");

    var checkoutPaymentButtonObserver = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            // Check for your element in the added nodes
            // var checkoutPaymentButton = document.getElementById("checkout-payment-continue");
            // customized for Školska Knjiga for now
            var formElement = document.querySelector('.payment-options');
            if (formElement) {
                if (!foundElement) {

                    // Clone the attributes of checkoutPaymentButton
                    var buttonInsideForm = formElement.querySelector('button');
                    customMonriButton.id = "custom-monri-button";
                    customMonriButton.textContent = "PAY";
                    customMonriButton.type = "button";
                    customMonriButton.addEventListener("click", function () {
                        customMonriButton.classList.add(isLoadingClassName);
                        getCheckoutData();
                    });

                    // Clone the classes and other attributes
                    Array.from(buttonInsideForm.classList).forEach(function (className) {
                        customMonriButton.classList.add(className);
                    });

                    // Insert the new button after checkoutPaymentButton
                    buttonInsideForm.insertAdjacentElement('afterend', customMonriButton);

                    // Disconnect the observer if needed
                    checkoutPaymentButtonObserver.disconnect();
                    foundElement = true;
                }
            }
        });
    });

    var radioButtonObserver = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            var radioButtonsCollection = document.getElementsByClassName("form-checklist optimizedCheckout-form-checklist");
            if (radioButtonsCollection.length > 0) {
                if (!foundRadioButtons) {
                    radioButtonsCollection = Array.from(radioButtonsCollection); // Convert to an array for easier handling

                    radioButtonsCollection.forEach(function (radioButtonContainer) {
                        var radioButtonCollection = radioButtonContainer.querySelectorAll('input[name="paymentProviderRadio"]');

                        radioButtonCollection.forEach(function (radioButtonElement) {
                            radioButtonElement.addEventListener('change', function () {
                                toggleButtonsVisibility(radioButtonElement.value);
                            });
                            if (radioButtonElement.checked) {
                                toggleButtonsVisibility(radioButtonElement.value);
                            }
                        });
                    });
                    foundRadioButtons = true;
                    // Disconnect the observer or perform other logic if needed
                    radioButtonObserver.disconnect();
                }
            }

        });
    });

    function toggleButtonsVisibility(isChecked) {
        var newButton = document.getElementById("custom-monri-button");
        var checkoutPaymentButton = document.getElementById("checkout-payment-continue");

        if (isChecked === 'check') {
            newButton.style.display = 'inline-block';
            checkoutPaymentButton.style.display = 'none';
        } else {
            newButton.style.display = 'none';
            checkoutPaymentButton.style.display = 'inline-block';
        }
    }

    // Start observing changes to the DOM
    checkoutPaymentButtonObserver.observe(document.body, {
        childList: true,
        subtree: true
    });

    radioButtonObserver.observe(document.body, {
        childList: true,
        subtree: true
    });

    function getCheckoutData() {
        const options = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        };

        fetch(window.location.origin + cartUrl, options)
            .then(
                response => response.json()
            )
            .then(
                response => {
                    const checkoutData = {
                        CheckoutID: response[0].id,
                        SiteUrl: window.location.origin
                    }
                    sendCheckoutData(checkoutData);
                }
            )
            .catch(
                err => {
                    customMonriButton.classList.remove(isLoadingClassName);
                    console.error(err);
                }
            );
    }

    function sendCheckoutData(checkoutData) {
        const options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(checkoutData)
        };
        fetch(monriApiUrl + '/checkout/init', options)
            .then(
                response => response.json()
            )
            .then(
                response => {
                    console.log(response.url);
                    window.location.href = response.url;
                }
            )
            .catch(
                err => {
                    customMonriButton.classList.remove(isLoadingClassName);
                    console.error(err);
                }
            );
    }
});
