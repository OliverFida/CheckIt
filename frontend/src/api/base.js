const axios = require('axios').default;

const SERVER_CONF = {
    ADDRESS: "10.0.5.57",
    PORT: "7061"
};

async function apiRequest(path, method, data){
    var returnVal = null;

    var token = await localStorage.getItem('loginToken');

    await axios({
        url: `https://${SERVER_CONF.ADDRESS}:${SERVER_CONF.PORT}/${path}`,
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

module.exports = {
    apiRequest
};