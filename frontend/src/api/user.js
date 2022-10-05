const apiRequest = require('./base').apiRequest;

async function changePassword(password){
  return await apiRequest("user/password", "PATCH", {
    password: password
  });
}

async function getUserData(){
  var response = await apiRequest(`user/${localStorage.getItem('loginUsername')}`, "GET", null);
  return response.data;
}

module.exports = {
    changePassword,
    getUserData
};