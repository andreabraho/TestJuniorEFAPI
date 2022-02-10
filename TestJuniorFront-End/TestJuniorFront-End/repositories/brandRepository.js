import Repository from "./Repository";

const resource = "/brand";
export default {
  getPage(page,pageSize) {
    return Repository.get(`${resource}/page/${page}/${pageSize}`);
  },

  getBrand(brandId) {
    return Repository.get(`${resource}/detail/${brandId}`);
  },

  createBrand(payload) {
    return Repository.post(`${resource}/insert`, payload);
  },
  deleteBrand(brandId){
    return Repository.delete(`${resource}/delete/${brandId}`);
  },
  editBrand(payload){
    return Repository.put(`${resource}/update`, payload);
  },
  getDataForEdit(brandId){
    return Repository.get(`${resource}/update/${brandId}`);
  },
  validateEmail(email){
    return Repository.get(`${resource}/ValidateMail/${email}`)
  }
};
