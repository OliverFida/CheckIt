import base from './base';
import md5 from 'md5';

async function login(username, password){
    var passwordHash = md5(password);

    var response = await base.apiRequest('login', "POST", {username: username, password: passwordHash});

    if(response.status === 200){
        localStorage.setItem("loginUsername", username);
        localStorage.setItem("loginToken", response.data.value.stringToken);
        localStorage.setItem("loginAdmin", response.data.value.role === "Admin" ? true : false);
        response = await base.apiRequest(`user/${username}`);
        if(response.status === 200){
            localStorage.setItem("loginFirstName", response.data.firstName);
            localStorage.setItem("loginLastName", response.data.lastname);
            localStorage.setItem("loginRefreshToken", passwordHash);
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