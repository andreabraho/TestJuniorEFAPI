import Repository from "./Repository";

const resource = "/inforequest";
export default {
  getPage() {
    return Repository.get(`${resource}/page/`);
  },

  getInfoRequest(infoRequestId) {
    return Repository.get(`${resource}/detail/${infoRequestId}`);
  },
 
};
