import Repository from "./Repository";

const resource = "/product";
export default {
  getPage(page,pageSize,brandId=0,orderBy=0,isAsc=false) {
    return Repository.get(`${resource}/page/${page}/${pageSize}/${brandId}/${orderBy}/${isAsc}`);
  },

  getProduct(productId) {
    return Repository.get(`${resource}/detail/${productId}`);
  },

  createProduct(payload) {
    return Repository.post(`${resource}/insert`, payload);
  },
  getDataForCreate(){
    return Repository.get(`${resource}/insert`);
  },
  getDataForUpdate(id){
    return Repository.get(`${resource}/update/${id}`);
  },
  deleteProduct(productId){
    return Repository.delete(`${resource}/delete/${productId}`);
  },
  editProduct(payload){
    payload.product.id=parseInt(payload.product.id);
    return Repository.put(`${resource}/update`, payload);
  }
};
