import Repository from "./Repository";

const resource = "/product";
export default {
  getPage(page,pageSize,brandId=0,orderBy=0,isAsc=false) {

    let config={
      params:{
        brandId:brandId,
        orderBy:orderBy,
        isAsc:isAsc
      }
    }



    return Repository.get(`${resource}/page/${page}/${pageSize}`,config);
  },

  getProduct(productId) {
    return Repository.get(`${resource}/detail/${productId}`);
  },
  deleteProduct(productId){
    return Repository.delete(`${resource}/delete/${productId}`);
  },
  
  getDataForCreate(){
    return Repository.get(`${resource}/insert`);
  },
  getDataForUpdate(id){
    return Repository.get(`${resource}/update/${id}`);
  },
  
  editProduct(payload){
    payload.product.id=parseInt(payload.product.id);
    return Repository.put(`${resource}/upsert`, payload);
  },
  createProduct(payload) {
    return Repository.post(`${resource}/upsert`, payload);
  },
  
};
