import axios from 'axios';


async function apiRequest(path, method, data){
    var SERVER_CONF = await (await fetch('./backend_config.json')).json();
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
    });

    return returnVal;
}
var exports = {
    apiRequest
};
export default exports;