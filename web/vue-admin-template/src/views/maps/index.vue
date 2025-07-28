<template>
  <div class="app-container">
    <div class="fixed-header">
      <el-row v-if="!isMobile">
        <el-col :span="24">
          <el-radio-group v-model="difficulty" @input="changeDifficulty">
            <el-radio-button label="">全部</el-radio-button>
            <el-radio-button label="T1">T1</el-radio-button>
            <el-radio-button label="T2">T2</el-radio-button>
            <el-radio-button label="T3">T3</el-radio-button>
            <el-radio-button label="T4">T4</el-radio-button>
            <el-radio-button label="T5">T5</el-radio-button>
            <el-radio-button label="T6">T6</el-radio-button>
            <el-radio-button label="T7">T7</el-radio-button>
            <el-radio-button label="T8">T8</el-radio-button>
            <el-radio-button label="T0">未分类</el-radio-button>
          </el-radio-group>
        </el-col>
      </el-row>
      <el-row v-else>
        <el-col :span="24">
          <el-select v-model="difficulty" placeholder="请选择" style="width: 100%;" @change="changeDifficulty">
            <el-option label="全部" value="" />
            <el-option label="T1" value="T1" />
            <el-option label="T2" value="T2" />
            <el-option label="T3" value="T3" />
            <el-option label="T4" value="T4" />
            <el-option label="T5" value="T5" />
            <el-option label="T6" value="T6" />
            <el-option label="T7" value="T7" />
            <el-option label="T8" value="T8" />
            <el-option label="未分类" value="T0" />
          </el-select>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="24">
          <el-input v-model="search" placeholder="请输入内容" class="input-with-select"
            :style="isMobile ? { width: '100%' } : { width: '612px' }" @keyup.enter.native="changeDifficulty">
            <el-button slot="append" icon="el-icon-search" @click="changeDifficulty" />
          </el-input>
        </el-col>
      </el-row>
    </div>
    <div class="scroll-body">
      <div class="card-flex-row">
        <el-card v-for="(item, idx) in data" :key="idx" :body-style="{ padding: '0px' }" class="card-item">
          <div style="cursor:pointer;" @click="openMap(item)">
            <div class="image-wrapper">
              <img class="image" :src="item.img">
            </div>
            <div style="padding: 14px;">
              <div class="card-info">
                <span>{{ item.difficulty }}</span>
                <span>{{ item.name }}</span>
              </div>
            </div>
          </div>
        </el-card>
      </div>
      <div class="noMore" v-show="noMore">
        —— 没有更多了 ——
      </div>
    </div>
  </div>
</template>

<script>
import { getMapList } from '@/api/maps'

export default {
  name: 'MapIndex',
  data() {
    return {
      data: [],
      pageIndex: 1,
      isMobile: window.innerWidth < 768,
      search: '',
      difficulty: '',
      list: null,
      listLoading: true,
      loadingMore: false,
      noMore: false
    }
  },
  created() {
    this.loadData()
  },
  mounted() {
    document.body.style.overflow = 'hidden'
    document.documentElement.style.overflow = 'hidden'
    this.$nextTick(() => {
      const scrollBody = this.$el.querySelector('.scroll-body')
      if (scrollBody) {
        scrollBody.addEventListener('scroll', this.handleScroll)
      }
      window.addEventListener('resize', this.handleResize)
    })
  },
  // 使用keep-alive时缓存每次进来触发
  activated() {
    document.body.style.overflow = 'hidden'
    document.documentElement.style.overflow = 'hidden'
    this.$nextTick(() => {
      const scrollBody = this.$el.querySelector('.scroll-body')
      if (scrollBody) {
        scrollBody.addEventListener('scroll', this.handleScroll)
      }
      window.addEventListener('resize', this.handleResize)
    })
  },
  deactivated() {
    document.body.style.overflow = ''
    document.documentElement.style.overflow = ''
    const scrollBody = this.$el.querySelector('.scroll-body')
    if (scrollBody) {
      scrollBody.removeEventListener('scroll', this.handleScroll)
    }
    window.removeEventListener('resize', this.handleResize)
  },
  methods: {
    changeDifficulty() {
      this.pageIndex = 1
      this.data = []
      this.noMore = false
      this.loadingMore = false
      this.loadData()
    },
    handleScroll(e) {
      const target = e.target
      if (target.scrollTop + target.clientHeight >= target.scrollHeight - 10) {
        this.loadData()
      }
    },
    loadData() {
      if (this.loadingMore || this.noMore) return
      this.loadingMore = true
      // 这里请求新数据，追加到data
      getMapList({ difficulty: this.difficulty, search: this.search, pageIndex: this.pageIndex }).then(response => {
        const newData = response.data
        if (newData.length === 0) {
          this.noMore = true
        } else {
          this.data = this.data.concat(newData)
          this.pageIndex += 1
        }
        this.loadingMore = false
        // 判断是否需要继续加载
        this.$nextTick(() => {
          const scrollBody = this.$el.querySelector('.scroll-body')
          if (
            scrollBody &&
            scrollBody.scrollHeight <= scrollBody.clientHeight &&
            !this.noMore
          ) {
            // 内容还没填满，继续加载
            this.loadData()
          }
        })
      })
    },
    openMap(row) {
      this.$router.push({
        path: `/maps/detail/${row.id}`,
        query: { tagName: row.name }
      })
    },
    handleResize() {
      this.isMobile = window.innerWidth < 768
    }
  }
}
</script>
<style lang="scss" scoped>
.noMore {
  text-align: center;
  color: #999;
  padding: 20px 0;
}

.app-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
  overflow: hidden; // 防止自身出现滚动条
}

.scroll-body {
  flex: 1 1 0;
  overflow-y: auto;
  min-height: 0;
  /* 保证内容不被挡住 */
  padding-top: 10px;
  padding-bottom: 40px;
}

.el-row {
  margin-bottom: 10px;
}

.card-flex-row {
  display: flex;
  flex-wrap: wrap;
  gap: 16px;
}

.card-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-item {
  width: 100%;
  margin-bottom: 16px;
  cursor: pointer;
}

.image-wrapper {
  position: relative;
  width: 100%;
  padding-top: 56.25%; // 16:9 = 9/16 = 0.5625
  overflow: hidden;
}

.image {
  position: absolute;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

@media (min-width: 380px) {
  .card-item {
    width: calc((100% - 16px) / 2);
  }
}

@media (min-width: 580px) {
  .card-item {
    width: calc((100% - 32px) / 3);
  }
}

@media (min-width: 780px) {
  .card-item {
    width: calc((100% - 48px) / 4);
  }
}

@media (min-width: 1000px) {
  .card-item {
    // 5列，4个16px间距
    width: calc((100% - 64px) / 5);
  }
}

@media (min-width: 1400px) {
  .card-item {
    width: calc((100% - 80px) / 6);
  }
}
</style>
