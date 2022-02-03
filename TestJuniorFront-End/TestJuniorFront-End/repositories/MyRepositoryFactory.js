import ProductRepository from "./productRepository";
import IRRepository from "./infoRequestRepository";

const repositories = {
  products: ProductRepository,
  inforequests: IRRepository
  // other repositories ...
};

export const MyRepositoryFactory = {
  get: name => repositories[name]
};
