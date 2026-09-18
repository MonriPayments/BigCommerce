// Configuration and constants
const monriApiUrl = 'https://monribigcomm.wspay.info';
const customerUrl = '/customer/current.jwt?app_client_id=';
var cartUrl = '/api/storefront/carts';
let customerTokens;
let service = null;
let module = null;
let hasWSPayMethodBeenSelected = false;
let clickCounter = 0;
let iFrameValuesToken = null;
let iFrameValuesNonToken = null;
let radioButtonIds = [];
let merchantIntegration = "";
let formButtonUrl = "";

// API functions
async function getMerchantIntegration() {
    const request = { SiteUrl: window.location.origin };
    const options = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(request)
    };
    try {
        const response = await fetch(`${monriApiUrl}/checkout/check-merchant-integration`, options);
        const data = await response.text();

        merchantIntegration = data;
        return data;
    } catch (err) {
        console.error(err);
    }
}

async function getClientId() {
    const options = { method: 'GET' };
    try {
        const response = await fetch(`${monriApiUrl}/users/clientId`, options);
        const data = await response.json();
        return data.clientId;
    } catch (err) {
        console.error(err);
    }
}

async function getLoggedCustomerJwtToken(clientId) {
    const options = { method: 'GET' };
    try {
        const response = await fetch(`${window.location.origin}${customerUrl}${clientId}`, options);
        try {
            const data = await response.text();
            return data;
        }
        catch (err) {
            return "";
        }

    } catch (err) {
        console.error(err);
    }
}

async function getCustomerTokensByEmail(jwtToken) {
    const bigcJwtToken = { jwt: jwtToken };
    const options = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(bigcJwtToken)
    };
    try {
        const response = await fetch(`${monriApiUrl}/users/customertokens`, options);
        const data = await response.json();
        return data;
    } catch (err) {
        console.error(err);
    }
}

// Checkout functions
async function initializeCustomerTokens() {
    try {
        const clientId = await getClientId();
        const customerJwtToken = await getLoggedCustomerJwtToken(clientId);

        if (!customerJwtToken.includes("error")) {
            customerTokens = await getCustomerTokensByEmail(customerJwtToken);
        } else {
            customerTokens = await getCustomerTokensByEmail("");
        }

    } catch (err) {
        console.error('Error initializing customer tokens:', err);
    }
}

async function getCartData() {
    const optionsGet = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    try {
        const response = await fetch(window.location.origin + cartUrl, optionsGet);
        const data = await response.json();
        return {
            CheckoutID: data[0].id,
            SiteUrl: window.location.origin
        };
    } catch (err) {
        console.error('Error fetching checkout data:', err);
        throw err;
    }
}

async function getIframeValuesFromApi(checkoutData) {
    const optionsPost = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(checkoutData)
    };

    try {
        const response = await fetch(monriApiUrl + '/checkout/get-iframe-values', optionsPost);
        const iframeValues = await response.json();
        return iframeValues;
    } catch (err) {
        console.error('Error fetching iframe values:', err);
        throw err;
    }
}

async function getFormUrlFromApi(checkoutData) {
    const optionsPost = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(checkoutData)
    };

    try {
        const response = await fetch(monriApiUrl + '/checkout/init', optionsPost);
        const formUrl = await response.json();
        return formUrl;
    } catch (err) {
        console.error('Error fetching formUrl:', err);
        throw err;
    }
}

function fetchLabelTextUntilSuccess(labelElement, maxRetries = 10, interval = 2000) {
    let attempts = 0;

    const intervalId = setInterval(async () => {
        try {
            const request = { SiteUrl: window.location.origin };
            const options = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(request)
            };
            const response = await fetch(`${monriApiUrl}/checkout/fetch-payment-method-name`, options);
            if (!response.ok) throw new Error("Network response was not ok");

            const data = await response.text();
            if (data) {
                labelElement.textContent = data;
                clearInterval(intervalId);
            }

        } catch (error) {
            console.warn(`Attempt ${attempts + 1} failed: ${error.message}`);
        }

        attempts++;
        if (attempts >= maxRetries) {
            clearInterval(intervalId);
            console.error("Max retries reached. Failed to fetch label text.");
            labelElement.textContent = `Plaæanje karticama`;
        }
    }, interval);
}

async function loadCheckout() {
    //module = await checkoutKitLoader.load('checkout-sdk');
    //service = module.createCheckoutService();
}

function startCheckout() {
    //return loadCheckout();
}

// DOM utility functions
function createElementWithAttributes(type, attributes) {
    if (attributes && attributes.id) {
        const existingElement = document.getElementById(attributes.id);
        if (existingElement) {
            return existingElement;
        }
    }

    const element = document.createElement(type);
    for (let key in attributes) {
        element.setAttribute(key, attributes[key]);
    }
    return element;
}

async function getFormUrl() {
    try {
        if (formButtonUrl.length < 1) {
            const checkoutData = await getCartData();
            let formUrl = await getFormUrlFromApi(checkoutData);
            formButtonUrl = formUrl.url;
        }
    } catch (err) {
        console.error('Error in getFormUrl:', err);
    }
}

async function getIframeValues() {

    const monriformfieldcontainer = document.getElementById("monriformfieldcontainer");
    const iframeValuesList = [];

    try {
        const checkoutData = await getCartData();
        let iframeValues = await getIframeValuesFromApi(checkoutData);
        iframeValuesList.push(iframeValues);
    } catch (err) {
        console.error('Error in getIframeValues:', err);
    }

    iframeValuesList[0].forEach((iframeValue, index) => {
        if (iframeValue.isTokenShopID === true) {
            iFrameValuesToken = iframeValue;
        }
        else {
            iFrameValuesNonToken = iframeValue;
        }
    })

    const inputs = [
        { name: 'shopID', value: iFrameValuesNonToken.shopID },
        { name: 'shoppingCartID', value: iFrameValuesNonToken.shoppingCartID },
        { name: 'version', value: iFrameValuesNonToken.version },
        { name: 'totalAmount', value: iFrameValuesNonToken.totalAmount },
        { name: 'signature', value: iFrameValuesNonToken.signature },
        { name: 'returnURL', value: iFrameValuesNonToken.returnURL },
        { name: 'cancelURL', value: iFrameValuesNonToken.cancelURL },
        { name: 'returnErrorURL', value: iFrameValuesNonToken.returnErrorURL },
        { name: 'iframe', value: iFrameValuesNonToken.iFrame },
        { name: 'iframeResponseTarget', value: iFrameValuesNonToken.iFrameResponseTarget },
        { name: 'isTokenRequest', value: iFrameValuesNonToken.isTokenRequest },
        { name: 'token', value: null },
        { name: 'tokenNumber', value: null },
        { name: 'customerFirstName', value: iFrameValuesNonToken.customerFirstName },
        { name: 'customerLastName', value: iFrameValuesNonToken.customerLastName },
        { name: 'customerCity', value: iFrameValuesNonToken.customerCity },
        { name: 'customerZIP', value: iFrameValuesNonToken.customerZIP },
        { name: 'customerCountry', value: iFrameValuesNonToken.customerCountry },
        { name: 'customerEmail', value: iFrameValuesNonToken.customerEmail },
        { name: 'customerPhone', value: iFrameValuesNonToken.customerPhone },
        { name: 'customerAddress', value: iFrameValuesNonToken.customerAddress }
    ];

    const wspayFormElement = createElementWithAttributes('form', {
        name: 'pay',
        action: iFrameValuesNonToken.url,
        method: 'POST',
        target: 'wspay-iframe'
    });

    inputs.forEach(({ name, value }) => {
        const input = createElementWithAttributes('input', {
            type: 'hidden',
            name,
            value,
            id: `wspayiframe${name}`
        });
        wspayFormElement.appendChild(input);
    });

    const inputSub = createElementWithAttributes('input', {
        id: 'iframepaymentbutton',
        type: 'submit',
        value: 'Pay',
        style: 'display: none;'
    });
    wspayFormElement.appendChild(inputSub);

    monriformfieldcontainer.appendChild(wspayFormElement);
    openForm();
}

// Dropdown functions
const dropdown = (printArea) => {
    const component = createElementWithAttributes('div', {
        style: 'position: relative; z-index: 9999; margin: 10px 20px; border: 1px solid #ddd'
    });

    const input = createInput();
    const dropdown = createCreditCardDdwList();

    component.appendChild(input);
    component.appendChild(dropdown);
    printArea.appendChild(component);
};

const createInput = () => {
    const input = document.createElement("div");
    input.classList = "input";
    input.addEventListener("click", toggleDropdown);

    const inputPlaceholder = document.createElement("div");
    inputPlaceholder.classList = "input__placeholder";

    const placeholder = document.createElement("span");
    placeholder.textContent = "Odaberi karticu";
    placeholder.classList.add('placeholder');

    inputPlaceholder.appendChild(placeholder);
    inputPlaceholder.appendChild(dropdownIcon());
    input.appendChild(inputPlaceholder);

    return input;
};

const createCreditCardDdwList = () => {
    const structure = document.createElement("div");
    structure.classList.add("structure", "hide");
    structure.style.position = 'absolute';

    customerTokens.customerTokens.forEach(user => {
        const { token, tokenNumber, maskedPan, imageSource } = user;
        const option = document.createElement("div");
        option.addEventListener("click", () => {
            selectOption(maskedPan, imageSource);
            openForm(token, tokenNumber);
        });
        option.setAttribute("id", token);

        const n = document.createElement("span");
        n.textContent = maskedPan;

        const t = document.createElement("img");
        t.src = imageSource;

        option.appendChild(n);
        option.appendChild(t);
        structure.appendChild(option);
    });
    return structure;
};

const toggleDropdown = () => {
    const dropdown = document.querySelector(".structure");
    dropdown.classList.toggle("hide");

    const input = document.querySelector(".input");
    input.classList.toggle("input__active");
};

const selectOption = (name, imageSource) => {
    const text = document.querySelector('.placeholder');
    const dropdownSvg = document.querySelector('#dropdownSvg');

    let img = dropdownSvg.querySelector('img');
    if (img) {
        img.src = imageSource;
    } else {
        img = document.createElement('img');
        img.src = imageSource;
        dropdownSvg.insertBefore(img, dropdownSvg.firstChild);
    }
    text.innerHTML = '';

    const textSpan = document.createElement('span');
    textSpan.textContent = name;
    text.appendChild(textSpan);
    toggleDropdown();
};

const dropdownIcon = () => {
    const dropdown = document.createElement('span');
    dropdown.innerHTML = `
    <svg width="14px" height="7px" viewBox="0 0 10 5" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">
        <g id="Delivery" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
        <g id="Transactions-(Landing)" transform="translate(-1360.000000, -29.000000)" fill="#CDCFD3" fill-rule="nonzero">
            <g id="Group-4" transform="translate(1360.000000, 29.000000)">
                <polygon id="Shape" points="0 0 5 5 10 0"></polygon>
            </g>
        </g>
        </g>
    </svg>`;
    dropdown.style.marginRight = '10px';
    dropdown.id = "dropdownSvg";
    return dropdown;
};

const openForm = (token = '', tokenNumber = '', shopID = '', signature = '', isTokenRequest = false) => {
    //Get current values of WSPay Iframe params, change token attributes, shopID and signature
    if (token.length === 0) {
        document.getElementById('saveCreditCardCheckbox').disabled = false;
        document.getElementById('wspayiframetoken').value = token;
        document.getElementById('wspayiframetokenNumber').value = tokenNumber;
        document.getElementById('wspayiframeshopID').value = iFrameValuesNonToken.shopID;
        document.getElementById('wspayiframesignature').value = iFrameValuesNonToken.signature;
        if (isTokenRequest == true) {
            document.getElementById('wspayiframeisTokenRequest').value = "1";
        } else {
            document.getElementById('wspayiframeisTokenRequest').value = "0";
        }
    } else {
        document.getElementById('wspayiframetoken').value = token;
        document.getElementById('wspayiframetokenNumber').value = tokenNumber;
        document.getElementById('wspayiframeshopID').value = iFrameValuesToken.shopID;
        document.getElementById('wspayiframesignature').value = iFrameValuesToken.signature;
        document.getElementById('saveCreditCardCheckbox').disabled = true;
    }

    document.getElementById('iframepaymentbutton').click();
    iFrameResize({ checkOrigin: false }, '#wspay-iframe')
}

// Event listeners
window.addEventListener('load', function () {
    initializeCustomerTokens().then(() => {
        // Additional initialization code if needed
    });

    getMerchantIntegration().then(() => {
        //getFormUrl();
    });

    const paymentOptionRadioButtonLabel = createElementWithAttributes('label', {
        for: 'radio-monri',
        class: 'form-label optimizedCheckout-form-label'
    });

    fetchLabelTextUntilSuccess(paymentOptionRadioButtonLabel);
    //paymentOptionRadioButtonLabel.textContent = "Plaæanje karticama";

    setTimeout(() => {

    }, 2000);

    let foundPaymentChildElement = false;
    let foundRadioButtons = false;

    const checkoutPaymentObserver = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            if (!foundPaymentChildElement) {
                const checkoutStepPaymentElement = document.querySelector('.checkout-step--payment');
                if (checkoutStepPaymentElement) {
                    const checkoutViewContentElement = checkoutStepPaymentElement.querySelector('.checkout-view-content');
                    if (checkoutViewContentElement) {
                        const checkoutPaymentHeader = document.querySelector('.checkout-step--payment .checkout-view-header');
                        const monriPaymentOptionContainer = createElementWithAttributes('div', { id: 'monripaymentformnew' });
                        checkoutPaymentHeader.parentNode.insertBefore(monriPaymentOptionContainer, checkoutPaymentHeader.nextSibling);

                        const monriPaymentCheckoutFormContainer = createElementWithAttributes('div', {
                            id: 'monripaymentcheckoutformcontainer',
                            class: 'checkout-form'
                        });

                        const monriFormElement = createElementWithAttributes('div', {
                            id: 'monriform'
                        });

                        const monriFormCheckoutItemContainer = createElementWithAttributes('div', {
                            id: 'monriformcheckoutitemcontainer',
                            class: 'optimizedCheckout-form-checklist-item'
                        });

                        const monriFormFieldContainer = createElementWithAttributes('div', {
                            style: 'border: 1px solid #ddd', class: 'form-field', id: "monriformfieldcontainer"
                        });

                        const paymentOptionRadioButton = createElementWithAttributes('input', {
                            id: 'radio-monri',
                            type: 'radio',
                            name: 'monriProviderRadio',
                            class: 'form-checklist-checkbox optimizedCheckout-form-checklist-checkbox',
                            value: 'monri-button'
                        });

                        clickCounter++;

                        if (clickCounter == 1) {
                            paymentOptionRadioButton.addEventListener('click', function () {
                                toggleButtonsVisibility('monri-button');
                                if (!document.getElementById('wspayiframeshopID') && merchantIntegration == "EmbeddedCheckout") {
                                    const radioButtonsCollection = document.getElementsByClassName("form-checklist optimizedCheckout-form-checklist");

                                    if (radioButtonsCollection.length > 0) {
                                        Array.from(radioButtonsCollection).forEach(function (radioButtonContainer) {
                                            const radioButtonCollection = radioButtonContainer.querySelectorAll('input[name="paymentProviderRadio"]');
                                            radioButtonCollection.forEach(function (radioButtonElement) {
                                                radioButtonElement.checked = false;
                                            });
                                        });
                                    }

                                    const checkoutPaymentButton = document.getElementById("checkout-payment-continue");
                                    if (checkoutPaymentButton) {
                                        checkoutPaymentButton.style.display = 'none';
                                    }


                                    document.getElementById('wspay-iframe').classList.remove('no-display');

                                    const dropdownElement = createElementWithAttributes('div', { id: 'content' });
                                    monriFormFieldContainer.insertBefore(dropdownElement, document.getElementById('wspay-iframe'));
                                    const printArea = document.querySelector("#content");
                                    dropdown(printArea);

                                    const saveCreditCardContainer = createElementWithAttributes('div', {
                                        id: 'saveCreditCardContainer',
                                        style: 'margin: 10px 20px'
                                    });
                                    const saveCreditCardCheckbox = createElementWithAttributes('input', {
                                        id: 'saveCreditCardCheckbox',
                                        type: 'checkbox',
                                        name: 'saveCreditCardCheckbox',
                                        style: 'display: inline-block;scale: 1.3;margin-right: 1rem;accent-color: #5068EB;vertical-align: -10%;'
                                    });

                                    const saveCreditCardCheckboxSpan = createElementWithAttributes('span', {
                                        id: 'saveCreditCardCheckboxSpan'
                                    });
                                    saveCreditCardCheckboxSpan.innerHTML = "Spremi karticu";

                                    const openFormOnCheckboxValueChange = () => {
                                        openForm('', '', '', '', saveCreditCardCheckbox.checked);
                                    };

                                    saveCreditCardCheckbox.addEventListener('click', () => {
                                        openFormOnCheckboxValueChange();
                                    });

                                    saveCreditCardCheckboxSpan.addEventListener('click', () => {
                                        if (!saveCreditCardCheckbox.disabled) {
                                            saveCreditCardCheckbox.checked = !saveCreditCardCheckbox.checked;
                                            openFormOnCheckboxValueChange();
                                        }
                                    });

                                    saveCreditCardContainer.appendChild(saveCreditCardCheckbox);
                                    saveCreditCardContainer.appendChild(saveCreditCardCheckboxSpan);
                                    monriFormFieldContainer.insertBefore(saveCreditCardContainer, document.getElementById('wspay-iframe'));

                                    hasWSPayMethodBeenSelected = true;

                                    getIframeValues();
                                }
                            });
                        }

                        const wspayIFrameElement = createElementWithAttributes('iframe', {
                            name: 'wspay-iframe',
                            id: 'wspay-iframe',
                            style: 'width: 100%;'
                        });
                        wspayIFrameElement.classList.add('no-border');
                        wspayIFrameElement.classList.add('no-display');

                        monriFormFieldContainer.appendChild(paymentOptionRadioButton);
                        monriFormFieldContainer.appendChild(paymentOptionRadioButtonLabel);

                        if (merchantIntegration == "EmbeddedCheckout") {
                            monriFormFieldContainer.appendChild(wspayIFrameElement);
                        }

                        monriFormCheckoutItemContainer.appendChild(monriFormFieldContainer);
                        monriFormElement.appendChild(monriFormCheckoutItemContainer);
                        monriPaymentCheckoutFormContainer.appendChild(monriFormElement);
                        checkoutViewContentElement.insertBefore(monriPaymentCheckoutFormContainer, checkoutViewContentElement.firstChild);
                        foundPaymentChildElement = true;

                        checkoutPaymentObserver.disconnect();

                        const linkElement = createElementWithAttributes('link', {
                            rel: 'stylesheet',
                            type: 'text/css',
                            href: 'https://monribigcomm.wspay.info/bigcommstylesheet.css'
                        });
                        document.head.appendChild(linkElement);
                    }
                }
            }
        });
    });

    const radioButtonObserver = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            const radioButtonsPaymentStepCollection = document.getElementsByClassName("checkout-step optimizedCheckout-checkoutStep checkout-step--payment");
            const radioButtonsCollection = [];

            for (let i = 0; i < radioButtonsPaymentStepCollection.length; i++) {
                const elements = radioButtonsPaymentStepCollection[i].getElementsByClassName("form-checklist optimizedCheckout-form-checklist");
                radioButtonsCollection.push(...elements);
            }

            if (radioButtonsCollection.length > 0 && !foundRadioButtons) {
                Array.from(radioButtonsCollection).forEach(function (radioButtonContainer) {
                    const radioButtonCollection = radioButtonContainer.querySelectorAll('input[name="paymentProviderRadio"]');
                    radioButtonCollection.forEach(function (radioButtonElement) {
                        if (radioButtonElement.id) {
                            radioButtonIds.push(radioButtonElement.id);
                        }

                        radioButtonElement.addEventListener('click', function () {
                            toggleButtonsVisibility(radioButtonElement.value);
                        });
                    });
                });
                foundRadioButtons = true;
                radioButtonObserver.disconnect();
            }
        });
    });

    function toggleButtonsVisibility(radioButtonValue) {
        const checkoutPaymentButton = document.getElementById("checkout-payment-continue");
        const monriRadioButton = document.getElementById('radio-monri');
        const monricheckoutPaymentButton = document.getElementById("monri-checkout-payment-continue");

        if (radioButtonValue !== 'monri-button') {
            checkoutPaymentButton.style.display = 'inline-block';
            monriRadioButton.checked = false;
            if (merchantIntegration == "EmbeddedCheckout") {
                document.getElementById('wspay-iframe').style.display = 'none';
                document.getElementById('saveCreditCardContainer').style.display = 'none';
                document.getElementById('content').style.display = 'none';
            } else {
                if (monricheckoutPaymentButton) {
                    monricheckoutPaymentButton.style.display = 'none';
                }
            }
        } else {
            if (checkoutPaymentButton) {
                checkoutPaymentButton.style.display = 'none';
            }

            if (!monricheckoutPaymentButton) {
                const clonedButton = checkoutPaymentButton.cloneNode(true);
                clonedButton.id = "monri-checkout-payment-continue";
                clonedButton.innerText = "PLATI";

                // Apply initial disabled styles
                clonedButton.disabled = true

                // Create a small loader spinner
                const loader = document.createElement("span");
                loader.classList.add("monri-loader");
                loader.style.position = "absolute";
                loader.style.right = "50%";
                loader.style.top = "50%";
                loader.style.transform = "translateY(-50%)";
                loader.style.width = "16px";
                loader.style.height = "16px";
                loader.style.border = "2px solid #fff";
                loader.style.borderTop = "2px solid #888";
                loader.style.borderRadius = "50%";
                loader.style.animation = "monri-spin 0.8s linear infinite";
                clonedButton.appendChild(loader);

                // Insert button after checkout button
                checkoutPaymentButton.parentNode.insertBefore(clonedButton, checkoutPaymentButton.nextSibling);

                // Wait until form URL is available, then enable button
                const enableButton = () => {
                    //clonedButton.disabled = false;
                    loader.style.display = "none";
                };

                // Function to handle the click
                clonedButton.addEventListener("click", () => {
                    if (formButtonUrl) {
                        if (formButtonUrl.length < 1) {
                            getFormUrl();
                        }
                    }
                    window.location.href = formButtonUrl;
                });

                // Simulate waiting for form URL to load
                // You can replace this interval logic with your actual event that sets formButtonUrl
                const checkUrl = setInterval(() => {
                    if (formButtonUrl && formButtonUrl.length > 0) {
                        enableButton();
                        clearInterval(checkUrl);
                    }
                }, 300);
            }

            if (merchantIntegration == "EmbeddedCheckout") {
                const wspayIframe = document.getElementById('wspay-iframe');
                if (wspayIframe) {
                    wspayIframe.style.display = 'block';
                }

                const saveCreditCardContainer = document.getElementById('saveCreditCardContainer');
                if (saveCreditCardContainer) {
                    saveCreditCardContainer.style.display = 'inline-block';
                }

                const content = document.getElementById('content');
                if (content) {
                    content.style.display = 'block';
                }
            } else {
                document.getElementById("monri-checkout-payment-continue").style.display = 'inline-block';
                getFormUrl();
            }

            radioButtonIds.forEach(function (id) {
                const element = document.getElementById(id);
                if (element) {
                    element.checked = false; // Set the checked property to false
                }
            });
        }
    }

    function initializePaymentObserver() {
        foundPaymentChildElement = false;
        foundRadioButtons = false;
        clickCounter = 0;

        checkoutPaymentObserver.observe(document.body, {
            childList: true,
            subtree: true
        });

        radioButtonObserver.observe(document.body, {
            childList: true,
            subtree: true
        });
    }

    const resetObserver = new MutationObserver((mutations) => {
        const paymentStepExists = document.querySelector('.checkout-step--payment');
        if (!paymentStepExists) return;
        const monriExists = document.getElementById('monripaymentcheckoutformcontainer');
        if (!monriExists) {
            initializePaymentObserver();
        }
    });

    resetObserver.observe(document.body, {
        childList: true,
        subtree: true
    });
});

// Start the checkout process
startCheckout();
