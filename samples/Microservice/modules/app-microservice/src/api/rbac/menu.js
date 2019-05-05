import axios from "@/libs/api.request";

export const getMenuList = data => {
  return axios.request({
    url: "rbac/menu/list",
    method: "post",
    data
  });
};

// create menu
export const createMenu = data => {
  return axios.request({
    url: "rbac/menu/create",
    method: "post",
    data
  });
};

//load menu
export const loadMenu = data => {
  return axios.request({
    url: "rbac/menu/edit/" + data.guid,
    method: "get"
  });
};

// edit menu
export const editMenu = data => {
  return axios.request({
    url: "rbac/menu/edit",
    method: "post",
    data
  });
};

// delete menu
export const deleteMenu = ids => {
  return axios.request({
    url: "rbac/menu/delete/" + ids,
    method: "get"
  });
};

// batch command
export const batchCommand = data => {
  return axios.request({
    url: "rbac/menu/batch",
    method: "get",
    params: data
  });
};

//load menu truee
export const loadMenuTree = (guid) => {
  let url = "rbac/menu/tree";
  if (guid != null) {
    url += "/" + guid;
  }
  return axios.request({
    url: url,
    method: "get"
  });
};
