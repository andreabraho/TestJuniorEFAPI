import Vue from 'vue'
import Router from 'vue-router'

import ProductList from "../src/components/Products/ProductList/ProductList.vue"
import NewProduct from "../src/components/Products/New/AddProduct.vue"
import EditProduct from "../src/components/Products/Edit/EditProduct.vue"
import DetailProduct from "../src/components/Products/Detail/DetailProduct.vue"

import BrandList from "../src/components/Brands/BrandList/BrandList.vue"
import NewBrand from "../src/components/Brands/New/AddBrand.vue"
import EditBrand from "../src/components/Brands/Edit/EditBrand.vue"
import DetailBrand from "../src/components/Brands/Detail/DetailBrand.vue"

import InfoRequestList from "../src/components/InfoRequests/InfoRequestList/InfoRequestList.vue"
import InfoRequestDetail from "../src/components/InfoRequests/Detail/IRDetail.vue"


Vue.use(Router)



export default new Router({

  routes: [

    {

      path: '/products',

      name: 'ProductList',

      component: ProductList

    },
    {

      path: '/products/new',

      name: 'NewProduct',

      component: NewProduct

    },
    {

      path: '/products/:id/edit',

      name: 'EditProduct',

      component: EditProduct

    },
    {

      path: '/products/:id',

      name: 'DetailProduct',

      component: DetailProduct

    },
    {

      path: '/brands',

      name: 'BrandList',

      component: BrandList

    },
    {

      path: '/brands/new',

      name: 'NewBrand',

      component: NewBrand

    },
    {

      path: '/brands/:id/edit',

      name: 'EditBrand',

      component: EditBrand

    },
    {

      path: '/brands/:id',

      name: 'DetailBrand',

      component: DetailBrand

    },
    {

      path: '/leads',

      name: 'InfoRequestList',

      component: InfoRequestList

    },
    {

      path: '/leads/:id',

      name: 'InfoRequestDetail',

      component: InfoRequestDetail

    }




  ]

})