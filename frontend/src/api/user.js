import base from './base';

async function changePassword(password){
  return await base.apiRequest("user/password", "PATCH", {
    password: password
  });
}

async function getUserData(){
  var response = await base.apiRequest(`user/${localStorage.getItem('loginUsername')}`, "GET", null);
  return response.data;
}

var exports = {
    changePassword,
    getUserData
};
export default exports;