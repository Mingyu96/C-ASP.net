//js file for shopping cart

    const shoppingCartImg = document.getElementById("shopping-cart-img");
    const login = document.getElementById("login");
    const register = document.getElementById("signup");
    const cartTotal = document.getElementById("cart-total");
    const qtyBox = document.getElementsByClassName("qtybox"); //input box
    const removeBtn = document.getElementsByClassName("remove-pdt");
    const checkoutBtn = document.getElementById("checkout");
    
    checkoutBtn.addEventListener("click", Checkout);


    //update subtotal and cart total
    UpdateDisplay();

    //for the modal display
    const closeBtn = document.getElementById("close");
    const closeBtn2 = document.getElementById("close2");
    const modalContainer = document.getElementById("modal");
    //const modalContainer2 = document.getElementById("modal2");


    //Add event listener for all quantity box
    for (let i = 0; i < qtyBox.length; i++) {
        qtyBox[i].onchange = CheckInput;
        removeBtn[i].addEventListener("click", RemoveProduct);
    }
    //update subtotal and cart total
    function UpdateDisplay() {
        let carttotal = 0;
        for (let i = 0; i < qtyBox.length; i++) {
            let quantityEle = qtyBox[i].value;
            let priceEle = qtyBox[i].parentNode.previousElementSibling.children[1].innerHTML;
            let subtotal = quantityEle * parseFloat(priceEle);
            //update subtotal
            let subtotalEle = qtyBox[i].parentNode.nextElementSibling.children[1];
            subtotalEle.innerHTML = subtotal;

            //update to cart total
            carttotal += subtotal;
        }
        cartTotal.innerHTML = carttotal;
    }
    //ajax request to remove product from cart
    function RemoveProduct(event) {
        let target = event.currentTarget;
        let xhr = new XMLHttpRequest();
        xhr.open("POST", "/cart/RemoveProduct");
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (this.readyState === XMLHttpRequest.DONE) {
                if (this.status !== 200) {
                    return;
                }
                let data = JSON.parse(this.responseText);
                if (data === null) {
                    alert("Update not successful");
                } else if (data.delete === true){
                        let itemContainer = target.parentNode.parentNode.parentNode;
                        let parentItemContainer = target.parentNode.parentNode.parentNode.parentNode;
                        parentItemContainer.removeChild(itemContainer);
                    UpdateDisplay();
                    if (data.deletecart === true) //when cart item =0, refresh the page
                    {
                        window.location.reload();
                    }
                    
                }
            }
        }

        let data = {
            "id": target.id
        };
        xhr.send(JSON.stringify(data));
    }


    //hiding display from navbar
    shoppingCartImg.style.display = "none";
    login.style.display = "none";
    register.style.display = "none";


    function CheckInput(event) {
        //check the input value
        let target = event.currentTarget;
        if (target.value <= 0) {
            target.value = 1;
            alert("Quantity cannot be lower than 1");
        }
        if (target.value > 99) {
            target.value = 99;
            alert("Quantity cannot be more than 99");
        }
        //send to ajax method to update db
        UpdateQuantity(target);

        //update subtotal and cart total
    }

    // Ajax request gor checkout function
    function Checkout(event) {
        let target = event.currentTarget;
        let cartid = target.getAttribute("cartid");

        let xhr = new XMLHttpRequest();
        xhr.open("POST", "/cart/checkout");
        xhr.setRequestHeader("Content-Type", "application/json");

        xhr.onreadystatechange = function () {
            if (this.readyState === XMLHttpRequest.DONE) {
                if (this.status !== 200) {
                    return;
                }
                let data = JSON.parse(this.responseText);
                if (data === null) {
                    alert("Checkout was unsuccessful");
                    return;
                }
                if (data === false) {
                    alert("Login to checkout");
                    window.location.replace("/Home/Index");
                    return;
                }
                else {
                    if (data === true) {
                        window.open("/MyPurchase/record");
                    }
                }
            }
        }
        let data = {
            "id": cartid
        };
        xhr.send(JSON.stringify(data));
    }


//Ajax request to update product quantity in cart
    function UpdateQuantity(target) {
        let xhr = new XMLHttpRequest();
        xhr.open("POST", "/cart/UpdateQuantity");
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (this.readyState === XMLHttpRequest.DONE) {
                if (this.status !== 200) {
                    return;
                }
                let data = JSON.parse(this.responseText);
                if (data !== null) {
                    //retrieve the quantity and price
                    if (data.success === true) {
                        UpdateDisplay();
                    }
                } else {
                    alert("did not update");
                }
            }
        }

        let data = {};
        data.Id = target.id;
        data.Quantity = parseInt(target.value);

        xhr.send(JSON.stringify(data));
    }


