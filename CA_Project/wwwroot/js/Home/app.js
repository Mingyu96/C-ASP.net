window.onload = function () {
    //for the modal display
    const closeBtn = document.getElementById("close");
    const closeBtn2 = document.getElementById("close2");
    const modalContainer = document.getElementById("modal");
    
 



    const loginBtn = document.getElementById("login");
    const signupBtn = document.getElementById("signup");
    const displayname = document.getElementById('displayname');
    const logout = document.getElementById("logout");
    const shoppingCartImg = document.getElementById("shopping-cart-img");
    const purchases = document.getElementById("MyPurchase");
    const account = document.getElementById("account");
    

    //hidding displays
    displayname.style.display = 'none';
    logout.style.display = "none";
    shoppingCartImg.style.display = "none";
    purchases.style.display = "none";
    account.style.display = 'none';

   
    //Add Event for element
    closeBtn.addEventListener('click', close);
    closeBtn2.addEventListener('click', close2);
    loginBtn.addEventListener('click', open);
    signupBtn.addEventListener('click', open2);

 

    function close() {
        modalContainer.style.display = "none";
    }

    function close2() {
        modalContainer2.style.display = "none";
    }
    function open() {
        modalContainer.style.display = "flex";
    }
    function open2() {
        modalContainer2.style.display = "flex";
    }

    let username = document.getElementById("username");
    let email = document.getElementById("email");

    email.onblur = function () {
        checkIsEmailUsed(email.value);
    }

    username.onblur = function () {
        checkIsUsernameUsed(username.value);
    }

    

}

function disableBackButton() {
    window.history.forward()
}

//Close pop up screen
function close() {
    modalContainer.style.display = "none";
}

//Close pop up screen
function close2() {
    modalContainer2.style.display = "none";
}

//Open Login popup screen
function open() {
    modalContainer.style.display = "flex";
}
function open2() {
    modalContainer2.style.display = "flex";
}