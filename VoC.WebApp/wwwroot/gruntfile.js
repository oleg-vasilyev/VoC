module.exports = function (grunt) {
        grunt.loadNpmTasks('grunt-contrib-less');
        grunt.loadNpmTasks('grunt-contrib-watch');
        grunt.loadNpmTasks('grunt-contrib-cssmin');
        grunt.loadNpmTasks('grunt-autoprefixer');

        grunt.initConfig({

            less: {
                development: {
                    files: {
                        'styles.css': 'styles.less'
                    }
                }
            },

            watch: {
                styles: {
                    files: ['*.less'],
                    tasks: ['less'],
                    options: {
                        nospawn: true
                    }
                }
            },

            cssmin: {
                target: {
                    files: [{
                        expand: true,
                        src: 'styles.css'
                    }]
                }
            },


            autoprefixer: {
                development: {
                    browsers: ['last 2 version', 'ie 9'],
                    expand: true,
                    flatten: true,
                    src: 'styles.css'
                }
            },
        });


        grunt.registerTask('default', ['less', 'watch']);
        grunt.registerTask('rel', ['less', 'autoprefixer', 'cssmin']);

    };