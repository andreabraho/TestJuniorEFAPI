import Repository from "./Repository";

const resource = "/inforequest";
export default {
  getPage(page,pageSize,brandSelected=0,prodNameSearch=null,isAsc=false,productId=0) {
    if(prodNameSearch=="null")
      prodNameSearch=null;

    let config={
      params:{
        brandId:brandSelected,
        prodNameSearch:prodNameSearch,
        isAsc:isAsc,
        productId:productId
      }
    }

    return Repository.get(`${resource}/page/${page}/${pageSize}`,config);
  },

  getInfoRequest(infoRequestId) {
    return Repository.get(`${resource}/detail/${infoRequestId}`);
  },
 
};
