// Configuration and constants
const monriApiUrl = 'https://monribigcomm.wspay.info';
const customerUrl = '/customer/current.jwt?app_client_id=';
var cartUrl = '/api/storefront/carts';
let customerTokens;
let service = null;
let module = null;
let hasWSPayMethodBeenSelected = false;
let clickCounter = 0;
let eventCounter = 0;

let iFrameValuesToken = null;
let iFrameValuesNonToken = null;

// API functions
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

async function loadCheckout() {
    // module = await checkoutKitLoader.load('checkout-sdk');
    // service = module.createCheckoutService();
}

function startCheckout() {
    return loadCheckout();
}

// DOM utility functions
function createElementWithAttributes(type, attributes) {
    const element = document.createElement(type);
    for (let key in attributes) {
        element.setAttribute(key, attributes[key]);
    }
    return element;
}

async function getIframeValues() {

    const monriformfieldcontainer = document.getElementById("monriformfieldcontainer");
    const iframeValuesList = [];

    try {
        const checkoutData = await getCartData();
        let iframeValues = await getIframeValuesFromApi(checkoutData);
        iframeValuesList.push(iframeValues);
    } catch (err) {
        customMonriButton.classList.remove(isLoadingClassName);
        console.error('Error in getIframeValues:', err);
    }

    // You can use iframeValues here if needed

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
    placeholder.textContent = "Select credit card";
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

    let foundPaymentChildElement = false;
    let foundRadioButtons = false;

    const checkoutPaymentObserver = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            if (!foundPaymentChildElement) {
                const checkoutStepPaymentElement = document.querySelector('.form .payment-options');

                if (checkoutStepPaymentElement) {
                    if (true) {
                        const checkoutPaymentHeader = document.querySelector('.payment-options__item');

                        const monriPaymentOptionContainerContent = checkoutPaymentHeader.cloneNode(false);
                        monriPaymentOptionContainerContent.classList.remove('payment-options__item');
                        //monriPaymentOptionContainerContent.classList.add('payment-options__content');

                        const monriPlaceholderElement = document.querySelector('input[name="payment-option"][value="monri"]');
                        const monriPlaceholderParentElement = monriPlaceholderElement.parentElement.parentElement.parentElement;
                        const contentChild = monriPlaceholderParentElement.querySelector('.payment-options__content');
                        contentChild.appendChild(monriPaymentOptionContainerContent);

                        const monriFormElement = createElementWithAttributes('div', {
                            id: 'monriform'
                        });

                        const monriFormCheckoutItemContainer = createElementWithAttributes('div', {
                            id: 'monriformcheckoutitemcontainer',
                            class: 'optimizedCheckout-form-checklist-item'
                        });

                        const monriFormFieldContainer = createElementWithAttributes('div', {
                            id: "monriformfieldcontainer"
                        });

                        clickCounter++;

                        if (clickCounter == 1) {

                            monriPlaceholderParentElement.addEventListener('click', function () {
                                //const contentChild = monriPlaceholderParentElement.querySelector('.payment-options__content');
                                //contentChild.classList.add('active');
                                eventCounter++;
                                if (!document.getElementById('wspayiframeshopID') && !document.getElementById('saveCreditCardCheckbox')) {

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
                                    saveCreditCardCheckboxSpan.innerHTML = "Save card for future payments";

                                    // Add click event listener to both the checkbox and the label
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
                            //,sandbox: 'allow-same-origin allow-scripts allow-forms allow-modals allow-popups allow-downloads allow-presentation allow-popups-to-escape-sandbox allow-top-navigation-by-user-activation'
                        });
                        wspayIFrameElement.classList.add('no-border');
                        wspayIFrameElement.classList.add('no-display');

                        monriFormFieldContainer.appendChild(wspayIFrameElement);

                        monriFormCheckoutItemContainer.appendChild(monriFormFieldContainer);
                        monriFormElement.appendChild(monriFormCheckoutItemContainer);
                        //monriPaymentCheckoutFormContainer.appendChild(monriFormElement);
                        monriPaymentOptionContainerContent.appendChild(monriFormElement);
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

    // const radioButtonObserver = new MutationObserver(function (mutations) {
    // mutations.forEach(function (mutation) {
    // const radioButtonsCollection = document.getElementsByClassName("payment-options__item");
    // if (radioButtonsCollection.length > 0 && !foundRadioButtons) {
    // Array.from(radioButtonsCollection).forEach(function (radioButtonContainer) {
    // radioButtonContainer.addEventListener('click', function () {
    // toggleButtonsVisibility(radioButtonContainer);
    // });
    // });
    // foundRadioButtons = true;
    // radioButtonObserver.disconnect();
    // }
    // });
    // });

    function toggleButtonsVisibility(radioButtonValue) {
        //const checkoutPaymentButton = document.getElementsByClassName("payment-options__button");
        //const monriRadioButton = radioButtonValue.querySelector('input[name="payment-option"]');

        //if (monriRadioButton.value !== 'monri') {
        //    checkoutPaymentButton[0].style.display = 'inline-block';
        //    document.getElementById('wspay-iframe').style.display = 'none';
        //} else {
        //    checkoutPaymentButton[0].style.display = 'none';
        //    openForm();
        //}
    }

    checkoutPaymentObserver.observe(document.body, {
        childList: true,
        subtree: true
    });

    // radioButtonObserver.observe(document.body, {
    // childList: true,
    // subtree: true
    // });
});

// Start the checkout process
startCheckout();
