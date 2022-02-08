import Repository from "./Repository";

const resource = "/inforequest";
export default {
  getPage(page,pageSize,brandSelected=0,prodNameSearch=null,isAsc=false) {
    return Repository.get(`${resource}/page/${page}/${pageSize}/${brandSelected}/${prodNameSearch}/${isAsc}`);
  },

  getInfoRequest(infoRequestId) {
    return Repository.get(`${resource}/detail/${infoRequestId}`);
  },
 
};
