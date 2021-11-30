window.onload = function () {
    const loginSpan = document.getElementById("login");
    const signupSpan = document.getElementById("signup");
    const shoppingCartImg = document.getElementById("shopping-cart-img");
    const numberOfItems = document.getElementById("number-of-items-cart");

    let items = parseInt(numberOfItems.innerHTML);
    if (items == 0) { numberOfItems.style.display = "none"; }
    else { numberOfItems.style.display = "inline";}

    loginSpan.style.display = "none";
    signupSpan.style.display = "none";
  
    const logout = document.getElementById("logout");
    const account = document.getElementById("account");
    const uname = document.getElementById("displayname")
    const purchase = document.getElementById("MyPurchase")
    let username = uname.innerHTML;
    if (username == "Hello, Guest") {
        loginSpan.style.display = "block";
        logout.style.display = "none";
        account.style.display = "none";
        purchase.style.display = "none";
    }
    else {
        loginSpan.style.display = "none";
    }

    const addCartBtns = document.getElementsByClassName("add-cart-btn");
    const alignmentContainer = document.getElementById("alignment-container");
    alignmentContainer.classList.add("center-align");

    for (let i = 0; i < addCartBtns.length; i++) {
        addCartBtns[i].addEventListener("click", AddToCart);
    }
    
    loginSpan.addEventListener('click', redirect_to_login);
 

    function AddToCart(event) {
        let target = event.currentTarget;
        AddToCartServer(target.id);
    }

    function AddToCartServer(id) {
        let xhr = new XMLHttpRequest();
        xhr.open("POST", "/producthome/AddToCart");
        xhr.setRequestHeader("Content-Type", "application/json;charset=utf8");

        xhr.onreadystatechange = function () {
            if (this.readyState == XMLHttpRequest.DONE) {
                if (this.status !== 200) {
                    return;
                }
                let data = JSON.parse(this.responseText);
                if (data.updated == true) {
                    UpdateSelectStatus();
                      //get the cart icon and increase the number
                    //let num;
                    //if (numberOfItems === null) {
                    //    window.location.href = "/producthome/index";
                    //    return;
                    //}
                    ////else {
                    //    num = parseInt(numberOfItems.innerHTML);
                    //}
                    //num += 1;
                    //numberOfItems.innerHTML = num;
                }
                if (data.updated == false)
                {
                    alert("Update Cart Failed")
                    if (data.limit == true) //Product quantity limit = 99, not able to increase after that
                    {
                        alert("Quantity exceed limit(99)")
                    }
                }
            }
        }

        let data = {
            "id": id
        };

        xhr.send(JSON.stringify(data));
    }
}
function UpdateSelectStatus() {
    let selectbox_elem = document.getElementById("number-of-items-cart");
    let count = parseInt(selectbox_elem.innerHTML);
    selectbox_elem.style.display = "inline";
    selectbox_elem.innerHTML = count + 1;
}


function redirect_to_login()
{ window.location.replace("/Home/Index") }
