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
  deleteProduct(productId){
    return Repository.delete(`${resource}/detail/${productId}`);
  },
  editProduct(payload){
    return Repository.put(`${resource}/update`, payload);
  }
};
