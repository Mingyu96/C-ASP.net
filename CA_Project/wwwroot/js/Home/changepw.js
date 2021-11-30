window.onload = function () {
    let form = document.getElementById("form2");
    let ele = document.getElementById("cpassword");
    let pw = document.getElementById("newpassword");
    let con = document.getElementById("confirmp");
    let but = document.getElementById("subbtn");
    const loginSpan = document.getElementById("login");
    const signupSpan = document.getElementById("signup");
    let check = document.getElementById("checkpass1")

    loginSpan.style.display = "none";
    signupSpan.style.display = "none";

    //Password and username length and input validation upon submit new password
    form.onsubmit = function () {
        let password = ele.value;
        let newpassword = pw.value;
        let confirm = con.value;
        if (password != "") {
            if (newpassword == "") {
                alert("Please enter your new Password");
                return false;
            }
            if (newpassword.length < 5 || newpassword.length > 20) {
                alert("Password should be between 5-20 characters.");
                check.style.display = "inline";
                return false;
            }
            if (newpassword != confirm) {
                alert("Please Confirm your password!");
                return false;
            }
            else if (newpassword === confirm) {
                checkUserId(password,newpassword);
            }
            return false;
        }
        else {
            alert("Please enter your password!");
            return false;
        };
    }
}
//Ajax request to check duplicate username in database
function checkUserId(id,np) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/account/validate/newpassword");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status != 200)
                return;

            let data = JSON.parse(this.responseText);
            if (data.isOkay === false) {
                alert("Your OldPassword is Invalid!");
                window.location.href = "/account/changepw";
            }
            if (data.isOkay === true) {
                alert("Change Password Success!");
                window.location.href = "/producthome/index";
            }
        }
    };
    let data = {
        "Username":np,
        "Password": id
    };
    xhr.send(JSON.stringify(data));
}