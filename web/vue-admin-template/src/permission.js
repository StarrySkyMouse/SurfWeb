import router from './router'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style

NProgress.configure({ showSpinner: false }) // NProgress Configuration

router.beforeEach(async(to, from, next) => {
  // start progress bar
  NProgress.start()
  // const accessRoutes = await store.dispatch('permission/generateRoutes', 'admin')
  // // dynamically add accessible routes
  // router.addRoutes(accessRoutes)
  next()
})
router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})
