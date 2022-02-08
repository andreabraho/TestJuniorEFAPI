import ProductRepository from "./productRepository";
import IRRepository from "./infoRequestRepository";
import BrandRepository from "./brandRepository"
const repositories = {
  products: ProductRepository,
  inforequests: IRRepository,
  brands:BrandRepository
  // other repositories ...
};

export const MyRepositoryFactory = {
  get: name => repositories[name]
};
