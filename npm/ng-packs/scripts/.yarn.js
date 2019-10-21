"use strict"

const child_process = require("child_process")
const { env } = process
const { parent } = module

const REQUIRE_ESM = "--require esm"
const REQUIRE_DOT_YARN = "--require ./.yarn.js"

let { NODE_OPTIONS } = env

if (typeof NODE_OPTIONS === "string") {
  NODE_OPTIONS += " "
} else {
  NODE_OPTIONS = ""
}

if (parent != null &&
    parent.id === "internal/preload") {
  env.NODE_OPTIONS = NODE_OPTIONS.replace(REQUIRE_DOT_YARN, REQUIRE_ESM)
} else {
  child_process.spawn("yarn", process.argv.slice(2), {
    env: Object.assign({}, env, {
      NODE_OPTIONS: REQUIRE_DOT_YARN + " " + NODE_OPTIONS
    }),
    stdio: "inherit"
  })
}
