import Repository from "./Repository";

const resource = "/product";
export default {
  getPage(page,pageSize) {
    return Repository.get(`${resource}/page/${page}/${pageSize}`);
  },

  getProduct(productId) {
    return Repository.get(`${resource}/detail/${productId}`);
  },

  createProduct(payload) {
    return Repository.post(`${resource}`, payload);
  }
};
