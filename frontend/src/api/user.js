import base from './base';
import md5 from 'md5';

async function changePassword(password){
  var passwordHash = md5(password);
  return await base.apiRequest("user/password", "PATCH", {
    password: passwordHash
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