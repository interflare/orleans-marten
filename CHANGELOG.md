# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [v0.0.1-alpha.2] - 2025-01-12
### :boom: BREAKING CHANGES
- due to [`337448e`](https://github.com/interflare/orleans-marten/commit/337448e7b4f4e336ebbe38ee3c2ee2126fc8dd6d) - include orleans service id as part of state id *(commit by [@crookm](https://github.com/crookm))*:

  Manually prepending the ids in the database with `<orleans service id>-` will restore them. The default Orleans service id is `default`.


### :sparkles: New Features
- [`337448e`](https://github.com/interflare/orleans-marten/commit/337448e7b4f4e336ebbe38ee3c2ee2126fc8dd6d) - **persistence**: include orleans service id as part of state id *(commit by [@crookm](https://github.com/crookm))*
- [`6467af1`](https://github.com/interflare/orleans-marten/commit/6467af1ee295a092180088da433b1c603dcf8770) - marten clustering *(commit by [@crookm](https://github.com/crookm))*


## [v0.0.1-alpha.1] - 2025-01-10
### :sparkles: New Features
- [`dc61cac`](https://github.com/interflare/orleans-marten/commit/dc61cac909e6ebc0e13aa7cf3ba5c92d385dedea) - marten grain state persistence *(commit by [@crookm](https://github.com/crookm))*

[v0.0.1-alpha.1]: https://github.com/interflare/orleans-marten/compare/v0.0.1-alpha.0...v0.0.1-alpha.1
[v0.0.1-alpha.2]: https://github.com/interflare/orleans-marten/compare/v0.0.1-alpha.1...v0.0.1-alpha.2
