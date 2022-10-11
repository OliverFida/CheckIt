const axios = require('axios').default;

const SERVER_CONF = {
    PROTOCOL: "https",
    // ADDRESS: "10.0.5.91",
    ADDRESS: "localhost",
    PORT: "7061"
};

// const SERVER_CONF = {
//     PROTOCOL: "http",
//     ADDRESS: "10.0.3.19",
//     PORT: "5000"
// };

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
    });

    return returnVal;
}

module.exports = {
    apiRequest
};