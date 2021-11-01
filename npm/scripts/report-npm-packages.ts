import axios from 'axios';
import { program } from 'commander';
import fse from 'fs-extra';
import * as path from 'path';

(async () => {
  initCommander();
  await checkPackages();
})();

function initCommander() {
  program
    .requiredOption('-v, --compareVersion <version>', 'version to compare')
    .requiredOption(
      '-ra, --reportApi <report-api-url>',
      'api url to report status'
    )
    .requiredOption(
      '-ak, --accessKey <access-key>',
      'access key to use for report api'
    )
    .requiredOption(
      '-tc, --targetChannel <target-channel>',
      'target channel to send message'
    );

  program.parse(process.argv);
}

async function checkPackages() {
  const pkgJsonPaths = [
    ...fse
      .readdirSync('../packs')
      .map((folder) => path.resolve(`../packs/${folder}/package.json`)),
    ...fse
      .readdirSync('../ng-packs/packages')
      .map((folder) =>
        path.resolve(`../ng-packs/packages/${folder}/package.json`)
      ),
    ,
  ];

  const packageNames = pkgJsonPaths
    .map((pkgJsonPath) => fse.readJSONSync(pkgJsonPath).name)
    .filter((pkg) => !!pkg);

  const { compareVersion } = program.opts();

  let discordMessage = `NPM Packages Publish Status\nVersion: **${compareVersion}**\n\n`;

  for (let i = 0; i < packageNames.length; i++) {
    const packageName = packageNames[i];

    const lastVersion = await axios
      .get(`https://registry.npmjs.org/${packageName}`)
      .then((res) => {
        const versions = Object.keys(res.data.time);
        return versions[versions.length - 1];
      });

    let message;
    if (lastVersion === compareVersion) {
      message = `${packageName}: :ballot_box_with_check:\n`;
    } else {
      message = `${packageName}: :x:\n`;
    }

    discordMessage += message;
    console.log(
      message.replace(':ballot_box_with_check:', '✔️').replace(':x', '✖️')
    );
  }

  const { reportApi, targetChannel, accessKey } = program.opts();

  await axios.post(reportApi, {
    accessKey,
    targetChannel,
    message: discordMessage,
  });
}
