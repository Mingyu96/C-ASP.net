window.onload = function () {
    let element = document.getElementById("id");
    let e = document.getElementById("password");
    let showpass = document.getElementById("show");
    element.onblur = function () {
        checkUserIdP(element.value, e.value);
    }
    //show password
    showpass.addEventListener("click", ClickShowPass);

    //Get element
    const account = document.getElementById("account");
    const purchase = document.getElementById("MyPurchase")
    account.style.display = "none";
    purchase.style.display = "none";

    //Get element
    const cartImage = document.getElementById("shopping-cart-img");
    const displayname = document.getElementById("displayname");
    const logout = document.getElementById("logout");
    const loginBtn = document.getElementById("login");
    loginBtn.type = "submit";

    //Add event listener and hide/display element
    loginBtn.addEventListener('click', function () { window.location.href = "/home"; })
    cartImage.style.display = "none";
    displayname.style.display = "none";
    logout.style.display = "none";

    //get element
    let form = document.getElementById("form");
    let elem = document.getElementById("id");
    let ele = document.getElementById("password");
    let con = document.getElementById("confirm");
    let ema = document.getElementById("email");
    let fn = document.getElementById("fullname");
    let check = document.getElementById("checkpass")


    //Password and username length and input validation upon submit
  form.onsubmit = function () {
        let userid = elem.value;
        let password = ele.value;
        let confirm = con.value;
        let email = ema.value;
        let fullname = fn.value;
      if (password != "" && userid != "") {
          if (userid.length < 5 || userid.length > 20) {
              alert("UserId should be between 5-20 characters.");
              return false;
          }
          else if (password.length < 5 || password.length > 20) {
              alert("Password should be between 5-20 characters.");
              check.style.display = "inline";
              return false;
          }
            else if (password != confirm) {
              alert("Please confirm your password");
                return false;
            }
            else if (email == "") {
                alert("Please enter your Email!");
                return false;
            }
            else if (fullname == "") {
                alert("Please enter your Fullname!");
                return false;
            }
            else if (password === confirm && email != "") {
                return true;
            }
            return false;
            
       }
        else {
            alert("Please enter your userId and password!");
            return false;
        }
    }
}

//check if username exist in database
function checkUserIdP(id, password) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/home/CheckUser");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status != 200)
                return;
            let data = JSON.parse(this.responseText);
            if (data.isOkay === false) {
                alert("UserID is unavailable");
                window.location.href = "/home/register";
            }
        }
    };
    let data = {
        username: id,
        password: password
    };
    xhr.send(JSON.stringify(data));
}

// show password check box
function ClickShowPass() {
    let checke = document.getElementById("password");
    let checkcon = document.getElementById("confirm");
    if (checke.type === "password") {
        checke.type = "text";
    } else {
        checke.type = "password";
    }
    if (checkcon.value != null) {
        if (checkcon.type === "password") {
            checkcon.type = "text";
        } else {
            checkcon.type = "password";
        }
    }
}