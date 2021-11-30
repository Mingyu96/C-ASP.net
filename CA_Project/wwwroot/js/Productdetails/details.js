var num_jia = document.getElementById("num-jia");
var num_jian = document.getElementById("num-jian");
var input_num = document.getElementById("input-num");
var total = document.getElementById("total")
var price = document.getElementById("pprice")
var price1 = parseInt(price.innerHTML)


//quantity increase button
num_jia.onclick = function () {

    input_num.value = parseInt(input_num.value) + 1;
           
    total.innerHTML = "Total: $" + input_num.value * price1;

    
}
//quantity decrease button
num_jian.onclick = function () {

    if (input_num.value <= 0) {
        input_num.value = 0;
        total.innerHTML = "Total: $" + input_num.value * price1;
    } else {

        input_num.value = parseInt(input_num.value) - 1;
        total.innerHTML = "Total: $" + input_num.value * price1;
    }

}



window.onload = function () {
    const bigimg = document.getElementById("big");
    const loginSpan = document.getElementById("login");
    const signupSpan = document.getElementById("signup");
    const shoppingCartImg = document.getElementById("shopping-cart-img");
    const numberOfItems = document.getElementById("number-of-items-cart");

    let items = parseInt(numberOfItems.innerHTML);
    if (items == 0) { numberOfItems.style.display = "none"; }
    else { numberOfItems.style.display = "inline"; }

    const logout = document.getElementById("logout");
    const account = document.getElementById("account");
    const uname = document.getElementById("displayname")
    const purchase = document.getElementById("MyPurchase")
    let username = uname.innerHTML;
    if (username == "Hello, Guest") {
        logout.style.display = "none";
        account.style.display = "none";
        purchase.style.display = "none";
    }
    loginSpan.style.display = "none";
    signupSpan.style.display = "none";
    bigimg.style.display = "none";
    

    const addCartBtns = document.getElementsByClassName("add-cart-btn");
    const alignmentContainer = document.getElementById("alignment-container");
    alignmentContainer.classList.add("center-align");


    addCartBtns[0].addEventListener("click", AddToCart);

   

    //Add to cart function
    function AddToCart(event) {
        
       
        let target = event.currentTarget;
        var num = parseInt(input_num.value)
        AddToCartServer(target.id, num);
        
    }

    //Ajax request to add item to cart
    function AddToCartServer(id, num) {
        
       
        let xhr = new XMLHttpRequest();
       
        xhr.open("POST","/Productdetails/AddToCart");
            xhr.setRequestHeader("Content-Type", "application/json;charset=utf8");

            xhr.onreadystatechange = function () {
                if (this.readyState == XMLHttpRequest.DONE) {
                    if (this.status !== 200) {
                        return;
                    }
                    let data = JSON.parse(this.responseText);
                    if (data.updated == true) {
                        UpdateSelectStatus();

                    }
                    if (data.updated == false) {
                        alert("Update Cart Failed")
                        if (data.limit == true) {
                            alert("Quantity exceed limit(99)") //Alert user if product quantity more than 99
                        }
                    }
                }
            }

            let data = {
                "id": id,
                "num": num
            };

            xhr.send(JSON.stringify(data));
        
    }
}

//Check input for quantity and update price
input_num.onchange = CheckInput;


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
    total.innerHTML = "Total: $" + input_num.value * price1;    
}

//update number of item
function UpdateSelectStatus() {
    let selectbox_elem = document.getElementById("number-of-items-cart");
    let count = parseInt(selectbox_elem.innerHTML);
    selectbox_elem.style.display = "inline";

    for (var n = 0; n < input_num.value; n++)
    {
        count = count + 1;
    }
    
    selectbox_elem.innerHTML = count ;
   /* selectbox_elem.innerHTML = (count + input_num.value);*/
}


// zoom in photo function
$(function () {
    
    calculateMaskWH();

    $('.leftView').on('mouseout', function () {
        $('.mask').css('display', 'none');
        $('.rightView').css('display', 'none');
    });
   
    $('.leftView').on('mouseover', function () {
        $('.mask').css('display', 'block');
        $('.rightView').css('display', 'block');
        $('.bigImg').css('display', 'block');
    });
     
    
    $('.leftView').on('mousemove', function (event) {
      
        var left = event.pageX - $(this).offset().left - $('.mask').width() / 2;
        var top = event.pageY - $(this).offset().top - $('.mask').height() / 2;
       
        if (left < 0) {
            left = 0
        } else if (left > $(this).width() - $('.mask').width()) {
            left = $(this).width() - $('.mask').width();
        }
       
        if (top < 0) {
            top = 0;
        } else if (top > $(this).height() - $('.mask').height()) {
            top = $(this).height() - $('.mask').height();
        }
        $('.mask').css({
            left: left + 'px',
            top: top + 'px'
        });
     
        var rate = $('.bigImg').width() / $('.leftView').width();
        $('.bigImg').css({
            left: -rate * left + 'px',
            top: -rate * top + 'px'
        });
    });
   
    function calculateMaskWH() {
        var width = $('.leftView').width() / $('.bigImg').width() * $('.rightView').width();
        var height = $('.leftView').height() / $('.bigImg').height() * $('.rightView').height();
        $('.mask').css({
            "width": width,
            "height": height
        });
    }
});