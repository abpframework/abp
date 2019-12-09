// ESM syntax is supported.
import execa from 'execa';

const versions = ['major', 'minor', 'patch', 'premajor', 'preminor', 'prepatch', 'prerelease'];
let nextSemanticVersion = (process.argv[2] || '').toLowerCase();

if (versions.indexOf(nextSemanticVersion) < 0) {
  console.log(
    "Please enter the next semantic version like this: 'npm run publish patch'. Available versions: " +
      JSON.stringify(versions),
  );

  process.exit(1);
}

(async () => {
  try {
    await execa('yarn', ['install-new-dependencies'], { stdout: 'inherit' });
  } catch (error) {
    console.error(error.stderr);
    process.exit(1);
  }

  process.exit(0);
})();
