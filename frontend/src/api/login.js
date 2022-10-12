import base from './base';

async function login(username, password){
    var response = await base.apiRequest('login', "POST", {username: username, password: password});

    if(response.status === 200){
        localStorage.setItem("loginUsername", username);
        localStorage.setItem("loginToken", response.data.value.stringToken);
        localStorage.setItem("loginAdmin", response.data.value.role === "Admin" ? true : false);
        response = await base.apiRequest(`user/${username}`);
        if(response.status === 200){
            localStorage.setItem("loginFirstName", response.data.firstName);
            localStorage.setItem("loginLastName", response.data.lastname);
        }
    }

    return response;
}

function logout(){
    localStorage.clear();
}

var exports = {
    login: login,
    logout: logout
};
export default exports;