import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'home',
    component: () => import('./views/Home.vue')
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('./views/Login.vue')
  },
  {
    path: '/dashboard',
    name: 'dashboard',
    component: () => import('./views/Dashboard.vue'),
    meta: { requiresAuth: true }
  }
  // Adaugă aici rute noi, de exemplu:
  // {
  //   path: '/about',
  //   name: 'about',
  //   component: () => import('./views/About.vue')
  // }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router 