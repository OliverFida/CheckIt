const apiRequest = require('./base').apiRequest;

async function login(username, password){
    var response = await apiRequest('login', "POST", {username: username, password: password});

    if(response.status === 200){
        await localStorage.setItem("loginUsername", username);
        await localStorage.setItem("loginToken", response.data.value.stringToken);
        await localStorage.setItem("loginAdmin", response.data.value.role === "Admin" ? true : false);
    }

    return response;
}

function logout(){
    localStorage.clear();
}

module.exports = {
    login,
    logout
};