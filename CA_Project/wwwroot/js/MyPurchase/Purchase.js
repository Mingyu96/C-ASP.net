window.onload = function () {
  

    //show & hide html element
    const cartImage = document.getElementById("shopping-cart-img");
    const displayname = document.getElementById("displayname");
    const logout = document.getElementById("logout");
    const login = document.getElementById("login");
    const signup = document.getElementById("signup");
    const MyPurchase = document.getElementById("MyPurchase");
    const numberOfItems = document.getElementById("number-of-items-cart");

    let items = parseInt(numberOfItems.innerHTML);

    if (items == 0) { numberOfItems.style.display = "none"; }
    else { numberOfItems.style.display = "inline"; }

    displayname.style.display = 'none';
    login.style.display = "none";
    signup.style.display = "none";
    MyPurchase.style.display = "none";


    

}
