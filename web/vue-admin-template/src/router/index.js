import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sidebar
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/404',
    component: () => import('@/views/404'),
    hidden: true
  },
  {
    path: '/serverList',
    component: Layout,
    redirect: '/serverList',
    children: [{
      path: '',
      name: 'ServerList',
      component: () => import('@/views/serverList/index'),
      meta: { title: '服务器', icon: 'serverList', affix: true }
    }]
  },
  {
    path: '/',
    component: Layout,
    redirect: '/ranking',
    children: [{
      path: 'ranking',
      name: 'Ranking',
      component: () => import('@/views/ranking/index'),
      meta: { title: '排行', icon: 'raking', affix: true }
    }]
  },
  {
    path: '/news',
    component: Layout,
    redirect: '/news/index',
    children: [{
      path: 'index',
      name: 'NewsIndex',
      component: () => import('@/views/news/index'),
      meta: { title: '新的', icon: 'new', affix: true }
    }]
  },
  {
    path: '/maps',
    component: Layout,
    redirect: '/maps/index',
    children: [{
      path: 'index',
      name: 'MapIndex',
      component: () => import('@/views/maps/index'),
      meta: { title: '地图', icon: 'map', affix: true }
    }]
  },
  {
    path: '/maps/detail/:id',
    component: Layout,
    children: [{
      path: '',
      name: 'Detail',
      component: () => import('@/views/maps/detail'),
      meta: { title: '地图详情' }
    }],
    hidden: true
  },
  {
    path: '/player/:id',
    component: Layout,
    children: [{
      path: '',
      name: 'Player',
      component: () => import('@/views/player/index'),
      meta: { title: '玩家详情' }
    }],
    hidden: true
  },
  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]

const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
