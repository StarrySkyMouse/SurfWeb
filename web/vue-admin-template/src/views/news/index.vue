<template>
  <el-row>
    <el-col :xs="24" :sm="12">
      <div class="app-container">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>新纪录</span>
          </div>
          <el-row>
            <el-col :span="24">
              <el-radio-group v-model="recordType" @input="getNewRecordData()">
                <el-radio-button label="0">主线</el-radio-button>
                <el-radio-button label="1">奖励</el-radio-button>
                <el-radio-button label="2">阶段</el-radio-button>
              </el-radio-group>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <el-table v-loading="newRecordLoading" :data="newRecordList" element-loading-text="Loading"
                borderfithighlight-current-row>
                <el-table-column align="center" label="#" width="80">
                  <template slot-scope="scope">
                    {{ scope.$index + 1 }}
                  </template>
                </el-table-column>
                <el-table-column align="center" :label="'地图/'+(recordType == 0 ? '难度' : '阶段')">
                  <template slot-scope="scope">
                    <el-link type="primary" @click="openMap(scope.row.mapId,scope.row.mapName)">{{ scope.row.mapName }}({{ scope.row.notes }})</el-link>
                  </template>
                </el-table-column>
                <el-table-column align="center" label="玩家">
                  <template slot-scope="scope">
                    <el-link type="primary" @click="openPlayer(scope.row)">{{ scope.row.playerName }}</el-link>
                  </template>
                </el-table-column>
                <el-table-column align="center" label="时间" width="110">
                  <template slot-scope="scope">
                    {{ scope.row.time }}
                  </template>
                </el-table-column>
                <el-table-column align="center" label="日期" width="100">
                  <template slot-scope="scope">
                    {{ scope.row.date }}
                  </template>
                </el-table-column>
              </el-table></el-col>
          </el-row>
        </el-card>
      </div>
    </el-col>
    <el-col :xs="24" :sm="12">
      <div class="app-container">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>新地图</span>
          </div>
          <el-table v-loading="newMapLoading" :data="newMapList" element-loading-text="Loading"
            borderfithighlight-current-row>
            <el-table-column align="center" label="#" width="100">
              <template slot-scope="scope">
                {{ scope.$index + 1 }}
              </template>
            </el-table-column>
            <el-table-column align="center" label="名称/难度">
              <template slot-scope="scope">
                <el-link type="primary" @click="openMap(scope.row.id,scope.row.name)">{{ scope.row.name }}({{ scope.row.difficulty }})</el-link>
              </template>
            </el-table-column>
            <el-table-column align="center" label="日期" width="180">
              <template slot-scope="scope">
                {{ scope.row.createTime }}
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </div>
    </el-col>
    <el-col :xs="24" :sm="12">
      <div class="app-container">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>热门地图</span>
          </div>
          <el-table v-loading="popularMapLoading" :data="popularMapList" element-loading-text="Loading"
            borderfithighlight-current-row>
            <el-table-column align="center" label="#" width="120">
              <template slot-scope="scope">
                {{ scope.$index + 1 }}
              </template>
            </el-table-column>
            <el-table-column align="center" label="名称/难度">
              <template slot-scope="scope">
                <el-link type="primary" @click="openMap(scope.row.id,scope.row.name)">{{ scope.row.name }}({{ scope.row.difficulty }})</el-link>
              </template>
            </el-table-column>
            <el-table-column align="center" label="完成人数" width="200">
              <template slot-scope="scope">
                <span>{{ scope.row.surcessNumber }}</span>
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </div>
    </el-col>
  </el-row>
</template>

<script>
import { getNewRecordList, getNewMapList, getPopularMapList } from '@/api/news'

export default {
  data() {
    return {
      recordType: 0,
      newRecordList: null,
      newRecordLoading: true,
      newMapList: null,
      newMapLoading: true,
      popularMapList: null,
      popularMapLoading: true
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    openPlayer(row) {
      this.$router.push({
        path: `/player/${row.playerId}`,
        query: { tagName: row.playerName }
      })
    },
    openMap(id, name) {
      this.$router.push({
        path: `/maps/detail/${id}`,
        query: { tagName: name }
      })
    },
    getNewRecordData() {
      this.newRecordLoading = true
      getNewRecordList({ recordType: this.recordType }).then(response => {
        this.newRecordList = response.data
        this.newRecordLoading = false
      })
    },
    getNewMapData() {
      this.newMapLoading = true
      getNewMapList().then(response => {
        this.newMapList = response.data
        this.newMapLoading = false
      })
    },
    getPopularMapData() {
      this.popularMapLoading = true
      getPopularMapList().then(response => {
        this.popularMapList = response.data
        this.popularMapLoading = false
      })
    },
    fetchData() {
      this.getNewRecordData()
      this.getNewMapData()
      this.getPopularMapData()
    }
  }
}
</script>
<style lang="scss" scoped>
.el-row {
  margin: 20px;
}

::v-deep {
  .el-card__body {
    padding: 0px;
  }

  .app-container {
    padding: 10px;
  }

  @media (max-width: 768px) {
    .app-container {
      padding: 0px;
    }
  }
}
</style>
