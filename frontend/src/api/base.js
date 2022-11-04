import axios from 'axios';

const DEV = true;

var SERVER_CONF = null;

async function init(){
    try{
        SERVER_CONF = await (await fetch('./backend_config.json')).json();
    }catch(e){}
}
init();

async function apiRequest(path, method, data){
    var username = await localStorage.getItem('loginUsername');
    var refreshToken = await localStorage.getItem('loginRefreshToken');

    if(refreshToken !== null && refreshToken !== undefined && path !== "login" && path !== "system/version"){
        var refreshResponse = await sendRequest('login', 'POST', {
            username: username,
            password: refreshToken
        });

        if(refreshResponse.status === 200){
            await localStorage.setItem('loginToken', refreshResponse.data.value.stringToken);
            if(refreshResponse.data.value.role === 'Admin'){
                await localStorage.setItem('loginAdmin', true);
            }else{
                await localStorage.setItem('loginAdmin', false);
            }
        }else{
            await localStorage.clear();
        }
    }

    return await sendRequest(path, method, data);
}

async function sendRequest(path, method, data){
    var returnVal = null;

    var url;
    if(DEV){
        url = `${SERVER_CONF.PROTOCOL}://${SERVER_CONF.ADDRESS}:${SERVER_CONF.PORT}/${path}`;
    }else{
        if(SERVER_CONF){
            url = `${SERVER_CONF.PROTOCOL}://${SERVER_CONF.ADDRESS}:${SERVER_CONF.PORT}/${path}`;
        }else{
            url = `/api/${path}`;
        }
    }

    var token = await localStorage.getItem('loginToken');

    await axios({
        url: url,
        method: method,
        data: data,
        headers: {
            Authorization: `Bearer ${token}`
        }
    })
    .then((response) => {
        returnVal = response;
    })
    .catch((error) => {
        returnVal = error;
    });
    
    return returnVal;
}

async function ping(){
    var response = await apiRequest('system/version', 'GET', null);
    
    if(response.status === 200) return true;
    return false;
}

var exports = {
    apiRequest,
    ping
};
export default exports;