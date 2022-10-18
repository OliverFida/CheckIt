import axios from 'axios';

var SERVER_CONF = {
    PROTOCOL: 'http',
    ADDRESS: 'localhost',
    PORT: '5131'
};

async function init(){
    try{
        SERVER_CONF = await (await fetch('./backend_config.json')).json();
    }catch(e){}
}
init();

async function apiRequest(path, method, data){
    var returnVal = null;
    
    var token = await localStorage.getItem('loginToken');
    await axios({
        url: `${SERVER_CONF.PROTOCOL}://${SERVER_CONF.ADDRESS}:${SERVER_CONF.PORT}/${path}`,
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
    var response = await apiRequest('system/version', 'GET', null, false);
    if(response.status === 200) return true;
    return false;
}

var exports = {
    apiRequest,
    ping
};
export default exports;